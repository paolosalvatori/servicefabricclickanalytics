// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

#region Using Directives



#endregion

namespace Microsoft.AzureCat.Samples.UserEmulator
{
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

    public partial class MainForm : Form
    {
        #region Public Constructor

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            this.ConfigureComponent();
            this.ReadConfiguration();
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
        private const string UrlCannotBeNull = "The service endpoint URL setting cannot be null. Check the configuration file.";

        #endregion

        #region Private Fields

        private CancellationTokenSource cancellationTokenSource;
        private readonly Random random = new Random((int) DateTime.Now.Ticks);
        private int activeSessions;

        #endregion

        #region Public Methods

        public void ConfigureComponent()
        {
            this.cboServiceEndpointUrl.AutoSize = false;
            this.cboServiceEndpointUrl.Size = new Size(this.cboServiceEndpointUrl.Size.Width, 24);
            this.txtUserCount.AutoSize = false;
            this.txtUserCount.Size = new Size(this.txtUserCount.Size.Width, 24);
            this.txtEventIntervalInMilliseconds.AutoSize = false;
            this.txtEventIntervalInMilliseconds.Size = new Size(this.txtEventIntervalInMilliseconds.Size.Width, 24);
        }

        public void HandleException(Exception ex)
        {
            if (string.IsNullOrEmpty(ex?.Message))
            {
                return;
            }
            this.WriteToLog(string.Format(CultureInfo.CurrentCulture, ExceptionFormat, ex.Message));
            if (!string.IsNullOrEmpty(ex.InnerException?.Message))
            {
                this.WriteToLog(string.Format(CultureInfo.CurrentCulture, InnerExceptionFormat, ex.InnerException.Message));
            }
        }

        #endregion

        #region Private Methods

        public static bool IsJson(string item)
        {
            if (item == null)
            {
                throw new ArgumentException("The item argument cannot be null.");
            }
            try
            {
                JToken obj = JToken.Parse(item);
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
            {
                return null;
            }
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private void ReadConfiguration()
        {
            try
            {
                string urlValue = ConfigurationManager.AppSettings[UrlParameter];
                if (!string.IsNullOrWhiteSpace(urlValue))
                {
                    string[] urls = urlValue.Split(',', ';');
                    foreach (string url in urls)
                    {
                        this.cboServiceEndpointUrl.Items.Add(url);
                    }
                    this.cboServiceEndpointUrl.SelectedIndex = 0;
                }
                else
                {
                    this.WriteToLog(UrlCannotBeNull);
                }

                int value;
                string setting = ConfigurationManager.AppSettings[UserCountParameter];
                this.txtUserCount.Text = int.TryParse(setting, out value)
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : DefaultUserNumber.ToString(CultureInfo.InvariantCulture);
                setting = ConfigurationManager.AppSettings[EventIntervalParameter];
                this.txtEventIntervalInMilliseconds.Text = int.TryParse(setting, out value)
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : DefaultEventIntervalInMilliseconds.ToString(CultureInfo.InvariantCulture);
                setting = ConfigurationManager.AppSettings[EventsPerUserSessionParameter];
                this.trackbarEventsPerUserSession.Value = int.TryParse(setting, out value)
                    ? value
                    : DefaultEventsPerUserSession;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void WriteToLog(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(this.InternalWriteToLog), message);
            }
            else
            {
                this.InternalWriteToLog(message);
            }
        }

        private void InternalWriteToLog(string message)
        {
            lock (this)
            {
                if (string.IsNullOrEmpty(message))
                {
                    return;
                }
                string[] lines = message.Split('\n');
                DateTime now = DateTime.Now;
                string space = new string(' ', 19);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == 0)
                    {
                        string line = string.Format(
                            DateFormat,
                            now.Hour,
                            now.Minute,
                            now.Second,
                            lines[i]);
                        this.lstLog.Items.Add(line);
                    }
                    else
                    {
                        this.lstLog.Items.Add(space + lines[i]);
                    }
                }
                this.lstLog.SelectedIndex = this.lstLog.Items.Count - 1;
                this.lstLog.SelectedIndex = -1;
            }
        }

        #endregion

        #region Event Handlers

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.lstLog.Items.Clear();
        }

        /// <summary>
        /// Saves the log to a text file
        /// </summary>
        /// <param name="sender">MainForm object</param>
        /// <param name="e">System.EventArgs parameter</param>
        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lstLog.Items.Count <= 0)
                {
                    return;
                }
                this.saveFileDialog.Title = SaveAsTitle;
                this.saveFileDialog.DefaultExt = SaveAsExtension;
                this.saveFileDialog.Filter = SaveAsFilter;
                this.saveFileDialog.FileName = string.Format(
                    LogFileNameFormat,
                    DateTime.Now.ToString(CultureInfo.CurrentUICulture).Replace('/', '-').Replace(':', '-'));
                if (this.saveFileDialog.ShowDialog() != DialogResult.OK ||
                    string.IsNullOrEmpty(this.saveFileDialog.FileName))
                {
                    return;
                }
                using (StreamWriter writer = new StreamWriter(this.saveFileDialog.FileName))
                {
                    foreach (object t in this.lstLog.Items)
                    {
                        writer.WriteLine(t as string);
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }

        private void logWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.splitContainer.Panel2Collapsed = !((ToolStripMenuItem) sender).Checked;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }

        private void lstLog_Leave(object sender, EventArgs e)
        {
            this.lstLog.SelectedIndex = -1;
        }

        private void button_MouseEnter(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                control.ForeColor = Color.White;
            }
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                control.ForeColor = SystemColors.ControlText;
            }
        }

        // ReSharper disable once FunctionComplexityOverflow
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            int width = (this.mainHeaderPanel.Size.Width - 80)/2;
            int halfWidth = (width - 16)/2;

            this.txtUserCount.Size = new Size(halfWidth, this.txtUserCount.Size.Height);
            this.txtEventIntervalInMilliseconds.Size = new Size(halfWidth, this.txtEventIntervalInMilliseconds.Size.Height);
            this.trackbarEventsPerUserSession.Size = new Size(width - 32, this.trackbarEventsPerUserSession.Size.Height);

            this.txtEventIntervalInMilliseconds.Location = new Point(32 + halfWidth, this.txtEventIntervalInMilliseconds.Location.Y);
            this.trackbarEventsPerUserSession.Location = new Point(32 + width, this.trackbarEventsPerUserSession.Location.Y);

            this.lblEventIntervalInMilliseconds.Location = new Point(32 + halfWidth, this.lblEventIntervalInMilliseconds.Location.Y);
            this.lblEventsPerUserSession.Location = new Point(32 + width, this.lblEventsPerUserSession.Location.Y);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.txtUserCount.SelectionLength = 0;
        }

        // ReSharper disable once FunctionComplexityOverflow
        private void btnStart_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (string.Compare(this.btnStart.Text, Start, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // Validate parameters
                    if (!this.ValidateParameters())
                    {
                        return;
                    }

                    // Start Devices
                    this.StartUserSessions();

                    // Change button text
                    this.btnStart.Text = Stop;
                }
                else
                {
                    // Stop Devices
                    this.StopDevices();

                    // Change button text
                    this.btnStart.Text = Start;
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(this.cboServiceEndpointUrl.Text))
            {
                this.WriteToLog(UrlCannotBeNull);
                return false;
            }
            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.lstLog.Items.Clear();
        }

        private void StartUserSessions()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            int userCount = this.txtUserCount.IntegerValue;
            string serviceEndpointUrl = this.cboServiceEndpointUrl.Text;
            int eventInterval = this.txtEventIntervalInMilliseconds.IntegerValue;
            int eventsPerUserSession = this.trackbarEventsPerUserSession.Value;
            CancellationToken cancellationToken = this.cancellationTokenSource.Token;
            string apiControllerUrl = Combine(serviceEndpointUrl, "api/pageview");

            // Create one task for each device
            for (int i = 1; i <= userCount; i++)
            {
                int id = i;

#pragma warning disable 4014
                Task.Run(
                    async () =>
#pragma warning restore 4014
                    {
                        string userId = $"user{id:000}";
                        Interlocked.Increment(ref this.activeSessions);

                        // Create HttpClient object used to send events to the event hub.
                        HttpClient httpClient = new HttpClient
                        {
                            BaseAddress = new Uri(serviceEndpointUrl)
                        };

                        // Sets ContentType header to application/json
                        httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        this.WriteToLog($"User[{userId}] HttpClient created.");

                        // Creates "start user session" payload
                        Payload payload = new Payload
                        {
                            UserId = userId,
                            UserEvent = new UserEvent
                            {
                                EventType = EventType.StartSession,
                                Timestamp = DateTime.Now
                            }
                        };

                        // Serializes the payload in string format
                        string json = JsonConvert.SerializeObject(payload);

                        // Creates HttpContent
                        StringContent postContent = new StringContent(json, Encoding.UTF8, "application/json");

                        // Creates custom headers
                        postContent.Headers.Add("userId", userId);

                        // Sends the "start user session payload"
                        HttpResponseMessage response = await httpClient.PostAsync(apiControllerUrl, postContent, cancellationToken);
                        response.EnsureSuccessStatusCode();

                        // Logs event
                        this.WriteToLog($"User[{userId}] session started. ");

                        int j = eventsPerUserSession;
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                // Creates "user event" payload
                                EventType eventType = (EventType) this.random.Next(2, 5);
                                payload = new Payload
                                {
                                    UserId = userId,
                                    UserEvent = new UserEvent
                                    {
                                        EventType = eventType,
                                        Timestamp = DateTime.Now,
                                        MousePosition = new Position
                                        {
                                            X = this.random.Next(1, 801),
                                            Y = this.random.Next(1, 1001)
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
                                response = await httpClient.PostAsync(apiControllerUrl, postContent, cancellationToken);
                                response.EnsureSuccessStatusCode();

                                // Logs event
                                this.WriteToLog($"User[{userId}] event [{JsonConvert.SerializeObject(payload.UserEvent)}] sent. ");

                                // Decrements the number of events yet to send
                                j = j - 1;
                                if (j == 0)
                                {
                                    break;
                                }
                            }
                            catch (HttpRequestException ex)
                            {
                                this.WriteToLog($"User[{userId}] Message send failed: [{ex.Message}]");
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
                        this.WriteToLog($"User[{userId}] session stopped. ");
                    }).ContinueWith(
                        t =>
                        {
                            if (t.IsFaulted && t.Exception != null)
                            {
                                this.HandleException(t.Exception);
                            }
                            Interlocked.Decrement(ref this.activeSessions);
                            if (this.activeSessions == 0)
                            {
                                this.Invoke((Action) (() => this.btnStart.Text = Start));
                            }
                        },
                        cancellationToken);
            }
        }

        private void StopDevices()
        {
            this.cancellationTokenSource?.Cancel();
        }

        private void grouperDeviceManagement_CustomPaint(PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(
                new Pen(SystemColors.ActiveBorder, 1),
                this.cboServiceEndpointUrl.Location.X - 1,
                this.cboServiceEndpointUrl.Location.Y - 1,
                this.cboServiceEndpointUrl.Size.Width + 1,
                this.cboServiceEndpointUrl.Size.Height + 1);
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
            Random random = new Random();
            return new string(
                Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void trackbarEventsPerUserSession_ValueChanged(object sender, decimal value)
        {
            this.lblEventsPerUserSessionValue.Text = this.trackbarEventsPerUserSession.Value.ToString();
        }

        #endregion
    }
}