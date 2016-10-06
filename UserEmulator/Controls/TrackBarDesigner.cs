#region Copyright

// //=======================================================================================
// // Microsoft Azure Customer Advisory Team  
// //
// // This sample is supplemental to the technical guidance published on the community
// // blog at http://blogs.msdn.com/b/paolos/. 
// // 
// // Author: Paolo Salvatori
// //=======================================================================================
// // Copyright � 2016 Microsoft Corporation. All rights reserved.
// // 
// // THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// // EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// // MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
// //=======================================================================================

#endregion

#region Using Directives

using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

// ReSharper disable once CheckNamespace
namespace Microsoft.AzureCat.Samples.UserEmulator
{
    /// <summary>
    ///     The Designer for the <see cref="CustomTrackBar" />.
    /// </summary>
    public class CustomTrackBarDesigner : ControlDesigner
    {
        /// <summary>
        ///     Returns the allowable design time selection rules.
        /// </summary>
        public override SelectionRules SelectionRules
        {
            get
            {
                var control = Control as CustomTrackBar;

                // Disallow vertical or horizontal sizing when AutoSize = True
                if ((control != null) && control.AutoSize)
                    if (control.Orientation == Orientation.Horizontal)
                        return base.SelectionRules & ~SelectionRules.TopSizeable & ~SelectionRules.BottomSizeable;
                    else //control.Orientation == Orientation.Vertical
                        return base.SelectionRules & ~SelectionRules.LeftSizeable & ~SelectionRules.RightSizeable;
                return base.SelectionRules;
            }
        }

        //Overrides
        /// <summary>
        ///     Remove Button and Control properties that are
        ///     not supported by the <see cref="CustomTrackBar" />.
        /// </summary>
        protected override void PostFilterProperties(IDictionary Properties)
        {
            Properties.Remove("AllowDrop");
            Properties.Remove("BackgroundImage");
            Properties.Remove("ContextMenu");

            Properties.Remove("Text");
            Properties.Remove("TextAlign");
            Properties.Remove("RightToLeft");
        }

        //Overrides
        /// <summary>
        ///     Remove Button and Control events that are
        ///     not supported by the <see cref="CustomTrackBar" />.
        /// </summary>
        protected override void PostFilterEvents(IDictionary events)
        {
            //Actions
            events.Remove("Click");
            events.Remove("DoubleClick");

            //Appearence
            events.Remove("Paint");

            //Behavior
            events.Remove("ChangeUICues");
            events.Remove("ImeModeChanged");
            events.Remove("QueryAccessibilityHelp");
            events.Remove("StyleChanged");
            events.Remove("SystemColorsChanged");

            //Drag Drop
            events.Remove("DragDrop");
            events.Remove("DragEnter");
            events.Remove("DragLeave");
            events.Remove("DragOver");
            events.Remove("GiveFeedback");
            events.Remove("QueryContinueDrag");
            events.Remove("DragDrop");

            //Layout
            events.Remove("Layout");
            events.Remove("Move");
            events.Remove("Resize");

            //Property Changed
            events.Remove("BackColorChanged");
            events.Remove("BackgroundImageChanged");
            events.Remove("BindingContextChanged");
            events.Remove("CausesValidationChanged");
            events.Remove("CursorChanged");
            events.Remove("FontChanged");
            events.Remove("ForeColorChanged");
            events.Remove("RightToLeftChanged");
            events.Remove("SizeChanged");
            events.Remove("TextChanged");

            base.PostFilterEvents(events);
        }
    }
}