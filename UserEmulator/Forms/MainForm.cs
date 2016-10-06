#region Copyright

// //=======================================================================================
// // Microsoft Azure Customer Advisory Team  
// //
// // This sample is supplemental to the technical guidance published on the community
// // blog at http://blogs.msdn.com/b/paolos/. 
// // 
// // Author: Paolo Salvatori
// //=======================================================================================
// // Copyright © 2016 Microsoft Corporation. All rights reserved.
// // 
// // THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// // EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// // MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
// //=======================================================================================

#endregion

#region Using Directives

using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AzureCat.Samples.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

// ReSharper disable once CheckNamespace
namespace Microsoft.AzureCat.Samples.UserEmulator
{
    public partial class MainForm : Form
    {
        #region Public Constructor

        /// <summary>
        ///     Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            ConfigureComponent();
            ReadConfiguration();
        }

        #endregion

        #region Private Constants

        //***************************
        // Formats
        //***************************
        private const string DateFormat = "<{0,2:00}:{1,2:00}:{2,2:00}> {3}";
        private const string ExceptionFormat = "Exception: {0}";
        private const string InnerExceptionFormat = "InnerException: {0}";
        private const string LogFileNameFormat = "DeviceSimulatorLog-{0}.txt";

        //***************************
        // Constants
        //***************************
        private const string SaveAsTitle = "Save Log As";
        private const string SaveAsExtension = "txt";
        private const string SaveAsFilter = "Text Documents (*.txt)|*.txt";
        private const string Start = "Start";
        private const string Stop = "Stop";

        //***************************
        // Configuration Parameters
        //***************************
        private const string UrlParameter = "url";
        private const string UserCountParameter = "userCount";
        private const string EventIntervalParameter = "eventInterval";
        private const string EventsPerUserSessionParameter = "eventsPerUserSession";

        //***************************
        // Configuration Parameters
        //***************************
        private const int DefaultUserNumber = 10;
        private const int DefaultEventsPerUserSession = 10;
        private const int DefaultEventIntervalInMilliseconds = 100;


        //***************************
        // Messages
        //***************************
        private const string UrlCannotBeNull =
            "The service endpoint URL setting cannot be null. Check the configuration file.";

        #endregion

        #region Private Fields

        private CancellationTokenSource cancellationTokenSource;
        private readonly Random random = new Random((int) DateTime.Now.Ticks);
        private int activeSessions;

        #endregion

        #region Public Methods

        public void ConfigureComponent()
        {
            cboServiceEndpointUrl.AutoSize = false;
            cboServiceEndpointUrl.Size = new Size(cboServiceEndpointUrl.Size.Width, 24);
            txtUserCount.AutoSize = false;
            txtUserCount.Size = new Size(txtUserCount.Size.Width, 24);
            txtEventIntervalInMilliseconds.AutoSize = false;
            txtEventIntervalInMilliseconds.Size = new Size(txtEventIntervalInMilliseconds.Size.Width, 24);
        }

        public void HandleException(Exception ex)
        {
            if (string.IsNullOrEmpty(ex?.Message))
                return;
            WriteToLog(string.Format(CultureInfo.CurrentCulture, ExceptionFormat, ex.Message));
            if (!string.IsNullOrEmpty(ex.InnerException?.Message))
                WriteToLog(string.Format(CultureInfo.CurrentCulture, InnerExceptionFormat, ex.InnerException.Message));
        }

        #endregion

        #region Private Methods

        public static bool IsJson(string item)
        {
            if (item == null)
                throw new ArgumentException("The item argument cannot be null.");
            try
            {
                var obj = JToken.Parse(item);
                return obj != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string IndentJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private void ReadConfiguration()
        {
            try
            {
                var urlValue = ConfigurationManager.AppSettings[UrlParameter];
                if (!string.IsNullOrWhiteSpace(urlValue))
                {
                    var urls = urlValue.Split(',', ';');
                    foreach (var url in urls)
                        cboServiceEndpointUrl.Items.Add(url);
                    cboServiceEndpointUrl.SelectedIndex = 0;
                }
                else
                {
                    WriteToLog(UrlCannotBeNull);
                }

                int value;
                var setting = ConfigurationManager.AppSettings[UserCountParameter];
                txtUserCount.Text = int.TryParse(setting, out value)
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : DefaultUserNumber.ToString(CultureInfo.InvariantCulture);
                setting = ConfigurationManager.AppSettings[EventIntervalParameter];
                txtEventIntervalInMilliseconds.Text = int.TryParse(setting, out value)
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : DefaultEventIntervalInMilliseconds.ToString(CultureInfo.InvariantCulture);
                setting = ConfigurationManager.AppSettings[EventsPerUserSessionParameter];
                trackbarEventsPerUserSession.Value = int.TryParse(setting, out value)
                    ? value
                    : DefaultEventsPerUserSession;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void WriteToLog(string message)
        {
            if (InvokeRequired)
                Invoke(new Action<string>(InternalWriteToLog), message);
            else
                InternalWriteToLog(message);
        }

        private void InternalWriteToLog(string message)
        {
            lock (this)
            {
                if (string.IsNullOrEmpty(message))
                    return;
                var lines = message.Split('\n');
                var now = DateTime.Now;
                var space = new string(' ', 19);

                for (var i = 0; i < lines.Length; i++)
                    if (i == 0)
                    {
                        var line = string.Format(
                            DateFormat,
                            now.Hour,
                            now.Minute,
                            now.Second,
                            lines[i]);
                        lstLog.Items.Add(line);
                    }
                    else
                    {
                        lstLog.Items.Add(space + lines[i]);
                    }
                lstLog.SelectedIndex = lstLog.Items.Count - 1;
                lstLog.SelectedIndex = -1;
            }
        }

        #endregion

        #region Event Handlers

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
        }

        /// <summary>
        ///     Saves the log to a text file
        /// </summary>
        /// <param name="sender">MainForm object</param>
        /// <param name="e">System.EventArgs parameter</param>
        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstLog.Items.Count <= 0)
                    return;
                saveFileDialog.Title = SaveAsTitle;
                saveFileDialog.DefaultExt = SaveAsExtension;
                saveFileDialog.Filter = SaveAsFilter;
                saveFileDialog.FileName = string.Format(
                    LogFileNameFormat,
                    DateTime.Now.ToString(CultureInfo.CurrentUICulture).Replace('/', '-').Replace(':', '-'));
                if ((saveFileDialog.ShowDialog() != DialogResult.OK) ||
                    string.IsNullOrEmpty(saveFileDialog.FileName))
                    return;
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                {
                    foreach (var t in lstLog.Items)
                        writer.WriteLine(t as string);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void logWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer.Panel2Collapsed = !((ToolStripMenuItem) sender).Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog();
        }

        private void lstLog_Leave(object sender, EventArgs e)
        {
            lstLog.SelectedIndex = -1;
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
                control.ForeColor = Color.White;
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control != null)
                control.ForeColor = SystemColors.ControlText;
        }

        // ReSharper disable once FunctionComplexityOverflow
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var width = (mainHeaderPanel.Size.Width - 80)/2;
            var halfWidth = (width - 16)/2;

            txtUserCount.Size = new Size(halfWidth, txtUserCount.Size.Height);
            txtEventIntervalInMilliseconds.Size = new Size(halfWidth, txtEventIntervalInMilliseconds.Size.Height);
            trackbarEventsPerUserSession.Size = new Size(width - 32, trackbarEventsPerUserSession.Size.Height);

            txtEventIntervalInMilliseconds.Location = new Point(32 + halfWidth,
                txtEventIntervalInMilliseconds.Location.Y);
            trackbarEventsPerUserSession.Location = new Point(32 + width, trackbarEventsPerUserSession.Location.Y);

            lblEventIntervalInMilliseconds.Location = new Point(32 + halfWidth,
                lblEventIntervalInMilliseconds.Location.Y);
            lblEventsPerUserSession.Location = new Point(32 + width, lblEventsPerUserSession.Location.Y);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            txtUserCount.SelectionLength = 0;
        }

        // ReSharper disable once FunctionComplexityOverflow
        private void btnStart_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (string.Compare(btnStart.Text, Start, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // Validate parameters
                    if (!ValidateParameters())
                        return;

                    // Start Devices
                    StartUserSessions();

                    // Change button text
                    btnStart.Text = Stop;
                }
                else
                {
                    // Stop Devices
                    StopDevices();

                    // Change button text
                    btnStart.Text = Start;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(cboServiceEndpointUrl.Text))
            {
                WriteToLog(UrlCannotBeNull);
                return false;
            }
            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
        }

        private void StartUserSessions()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var userCount = txtUserCount.IntegerValue;
            var serviceEndpointUrl = cboServiceEndpointUrl.Text;
            var eventInterval = txtEventIntervalInMilliseconds.IntegerValue;
            var eventsPerUserSession = trackbarEventsPerUserSession.Value;
            var cancellationToken = cancellationTokenSource.Token;
            var apiControllerUrl = Combine(serviceEndpointUrl, "api/pageview");

            // Create one task for each device
            for (var i = 1; i <= userCount; i++)
            {
                var id = i;

#pragma warning disable 4014
                Task.Run(
                    async () =>
#pragma warning restore 4014
                        {
                            string userId = $"user{id:000}";
                            Interlocked.Increment(ref activeSessions);

                            // Create HttpClient object used to send events to the event hub.
                            var httpClient = new HttpClient
                            {
                                BaseAddress = new Uri(serviceEndpointUrl)
                            };

                            // Sets ContentType header to application/json
                            httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            httpClient.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
                            WriteToLog($"User[{userId}] HttpClient created.");

                            // Creates "start user session" payload
                            var payload = new Payload
                            {
                                UserId = userId,
                                UserEvent = new UserEvent
                                {
                                    EventType = EventType.StartSession,
                                    Timestamp = DateTime.Now
                                }
                            };

                            // Serializes the payload in string format
                            var json = JsonConvert.SerializeObject(payload);

                            // Creates HttpContent
                            var postContent = new StringContent(json, Encoding.UTF8, "application/json");

                            // Creates custom headers
                            postContent.Headers.Add("userId", userId);

                            // Sends the "start user session payload"
                            var response = await httpClient.PostAsync(apiControllerUrl, postContent, cancellationToken);
                            response.EnsureSuccessStatusCode();

                            // Logs event
                            WriteToLog($"User[{userId}] session started. ");

                            var j = eventsPerUserSession;
                            while (!cancellationToken.IsCancellationRequested)
                            {
                                try
                                {
                                    // Creates "user event" payload
                                    var eventType = (EventType) random.Next(2, 5);
                                    payload = new Payload
                                    {
                                        UserId = userId,
                                        UserEvent = new UserEvent
                                        {
                                            EventType = eventType,
                                            Timestamp = DateTime.Now,
                                            MousePosition = new Position
                                            {
                                                X = random.Next(1, 801),
                                                Y = random.Next(1, 1001)
                                            },
                                            EnteredText = eventType == EventType.TextEntered
                                                ? RandomString(5)
                                                : null
                                        }
                                    };

                                    // Serializes the payload in string format
                                    json = JsonConvert.SerializeObject(payload);

                                    // Creates HttpContent
                                    postContent = new StringContent(json, Encoding.UTF8, "application/json");

                                    // Creates custom headers
                                    postContent.Headers.Add("userId", userId);

                                    // Sends the "stop user session payload"
                                    response =
                                        await httpClient.PostAsync(apiControllerUrl, postContent, cancellationToken);
                                    response.EnsureSuccessStatusCode();

                                    // Logs event
                                    WriteToLog(
                                        $"User[{userId}] event [{JsonConvert.SerializeObject(payload.UserEvent)}] sent. ");

                                    // Decrements the number of events yet to send
                                    j = j - 1;
                                    if (j == 0)
                                        break;
                                }
                                catch (HttpRequestException ex)
                                {
                                    WriteToLog($"User[{userId}] Message send failed: [{ex.Message}]");
                                }

                                // Wait for the event time interval
                                Thread.Sleep(eventInterval);
                            }

                            // Creates "stop user session" payload
                            payload = new Payload
                            {
                                UserId = userId,
                                UserEvent = new UserEvent
                                {
                                    EventType = EventType.StopSession,
                                    Timestamp = DateTime.Now
                                }
                            };

                            // Serializes the payload in string format
                            json = JsonConvert.SerializeObject(payload);

                            // Creates HttpContent
                            postContent = new StringContent(json, Encoding.UTF8, "application/json");

                            // Create custom headers
                            postContent.Headers.Add("userId", userId);

                            // Send the "stop user session payload"
                            response = await httpClient.PostAsync(apiControllerUrl, postContent, cancellationToken);
                            response.EnsureSuccessStatusCode();

                            // Logs event
                            WriteToLog($"User[{userId}] session stopped. ");
                        }).ContinueWith(
                    t =>
                    {
                        if (t.IsFaulted && (t.Exception != null))
                            HandleException(t.Exception);
                        Interlocked.Decrement(ref activeSessions);
                        if (activeSessions == 0)
                            Invoke((Action) (() => btnStart.Text = Start));
                    },
                    cancellationToken);
            }
        }

        private void StopDevices()
        {
            cancellationTokenSource?.Cancel();
        }

        private void grouperDeviceManagement_CustomPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(
                new Pen(SystemColors.ActiveBorder, 1),
                cboServiceEndpointUrl.Location.X - 1,
                cboServiceEndpointUrl.Location.Y - 1,
                cboServiceEndpointUrl.Size.Width + 1,
                cboServiceEndpointUrl.Size.Height + 1);
        }

        #endregion

        #region Private Static Methods

        public static string Combine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return $"{uri1}/{uri2}";
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void trackbarEventsPerUserSession_ValueChanged(object sender, decimal value)
        {
            lblEventsPerUserSessionValue.Text = trackbarEventsPerUserSession.Value.ToString();
        }

        #endregion
    }
}