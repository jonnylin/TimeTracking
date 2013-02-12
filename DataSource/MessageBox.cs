﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DataSource
{
    public class MessageBox
    {
        public static async Task<MessageBoxResult> ShowAsyncModified(string msg, string cap, string optionOne, string optionTwo)
        {
            MessageBoxResult i = new MessageBoxResult();

            // Create the message dialog and set its content and title
            var messageDialog = new MessageDialog("New updates have been found for this program. Would you like to install the new updates?", "Updates available");

            // Add commands and set their callbacks
            messageDialog.Commands.Add(new UICommand(optionOne, (command) =>
            {
                i = MessageBoxResult.Working;
            }));

            messageDialog.Commands.Add(new UICommand(optionTwo, (command) =>
            {
                i = MessageBoxResult.NonWorking;
            }));

            messageDialog.Commands.Add(new UICommand("Cancel", (command) =>
            {
                i = MessageBoxResult.Cancel;
            }));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = (uint)MessageBoxResult.Cancel;

            // Show the message dialog
            await messageDialog.ShowAsync();

            return i;
        }


        public static async Task<MessageBoxResult> ShowAsync(string messageBoxText,
                                                             string caption,
                                                             MessageBoxButton button)
        {

            MessageDialog md = new MessageDialog(messageBoxText, caption);
            MessageBoxResult result = MessageBoxResult.None;
            if (button.HasFlag(MessageBoxButton.OK))
            {
                md.Commands.Add(new UICommand("OK",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.OK)));
            }
            if (button.HasFlag(MessageBoxButton.Yes))
            {
                md.Commands.Add(new UICommand("Yes",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.Yes)));
            }
            if (button.HasFlag(MessageBoxButton.No))
            {
                md.Commands.Add(new UICommand("No",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.No)));
            }
            if (button.HasFlag(MessageBoxButton.Cancel))
            {
                md.Commands.Add(new UICommand("Cancel",
                    new UICommandInvokedHandler((cmd) => result = MessageBoxResult.Cancel)));
                md.CancelCommandIndex = (uint)md.Commands.Count - 1;
            }
            var op = await md.ShowAsync();
            return result;
        }

        public static async Task<MessageBoxResult> ShowAsync(string messageBoxText)
        {
            return await MessageBox.ShowAsync(messageBoxText, null, MessageBoxButton.OK);
        }
    }

    // Summary:
    //     Specifies the buttons to include when you display a message box.
    [Flags]
    public enum MessageBoxButton
    {
        // Summary:
        //     Displays only the OK button.
        OK = 1,
        // Summary:
        //     Displays only the Cancel button.
        Cancel = 2,
        //
        // Summary:
        //     Displays both the OK and Cancel buttons.
        OKCancel = OK | Cancel,
        // Summary:
        //     Displays only the OK button.
        Yes = 4,
        // Summary:
        //     Displays only the Cancel button.
        No = 8,
        //
        // Summary:
        //     Displays both the OK and Cancel buttons.
        YesNo = Yes | No,
        YesNoCancel = Yes | No | Cancel,
    }

    // Summary:
    //     Represents a user's response to a message box.
    public enum MessageBoxResult
    {
        // Summary:
        //     This value is not currently used.
        None = 0,
        //
        // Summary:
        //     The user clicked the OK button.
        OK = 1,
        //
        // Summary:
        //     The user clicked the Cancel button or pressed ESC.
        Cancel = 2,

        Working = 3,
        NonWorking = 4,
        //
        // Summary:
        //     This value is not currently used.
        Yes = 6,
        //
        // Summary:
        //     This value is not currently used.
        No = 7,
    }
}
