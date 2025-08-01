﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace Microsoft.CmdPal.Ext.System.Helpers;

public class SettingsManager : JsonSettingsManager, ISettingsInterface
{
    private static readonly string _namespace = "system";

    private static string Namespaced(string propertyName) => $"{_namespace}.{propertyName}";

    private readonly ToggleSetting _showDialogToConfirmCommand = new(
        Namespaced(nameof(ShowDialogToConfirmCommand)),
        Resources.confirm_system_commands,
        Resources.confirm_system_commands,
        false); // TODO -- double check default value

    private readonly ToggleSetting _showSuccessMessageAfterEmptyingRecycleBin = new(
        Namespaced(nameof(ShowSuccessMessageAfterEmptyingRecycleBin)),
        Resources.Microsoft_plugin_sys_RecycleBin_ShowEmptySuccessMessage,
        Resources.Microsoft_plugin_sys_RecycleBin_ShowEmptySuccessMessage,
        false); // TODO -- double check default value

    private readonly ToggleSetting _hideEmptyRecycleBin = new(
        Namespaced(nameof(HideEmptyRecycleBin)),
        Resources.Microsoft_plugin_sys_RecycleBin_HideEmpty,
        Resources.Microsoft_plugin_sys_RecycleBin_HideEmpty,
        false);

    private readonly ToggleSetting _hideDisconnectedNetworkInfo = new(
        Namespaced(nameof(HideDisconnectedNetworkInfo)),
        Resources.Microsoft_plugin_ext_settings_hideDisconnectedNetworkInfo,
        Resources.Microsoft_plugin_ext_settings_hideDisconnectedNetworkInfo,
        false);

    internal static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        Directory.CreateDirectory(directory);

        // now, the state is just next to the exe
        return Path.Combine(directory, "settings.json");
    }

    public bool ShowDialogToConfirmCommand() => _showDialogToConfirmCommand.Value;

    public bool ShowSuccessMessageAfterEmptyingRecycleBin() => _showSuccessMessageAfterEmptyingRecycleBin.Value;

    public bool HideEmptyRecycleBin() => _hideEmptyRecycleBin.Value;

    public bool HideDisconnectedNetworkInfo() => _hideDisconnectedNetworkInfo.Value;

    public FirmwareType GetSystemFirmwareType() => Win32Helpers.GetSystemFirmwareType();

    public SettingsManager()
    {
        FilePath = SettingsJsonPath();

        Settings.Add(_showDialogToConfirmCommand);
        Settings.Add(_showSuccessMessageAfterEmptyingRecycleBin);
        Settings.Add(_hideEmptyRecycleBin);
        Settings.Add(_hideDisconnectedNetworkInfo);

        // Load settings from file upon initialization
        LoadSettings();

        Settings.SettingsChanged += (s, a) => this.SaveSettings();
    }
}
