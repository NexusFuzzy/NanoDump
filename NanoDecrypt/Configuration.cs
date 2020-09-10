using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoDecrypt
{
    class Configuration
    {
		private DateTime _BuildTime;
		public DateTime BuildTime
		{
			get { return _BuildTime; }
			set { _BuildTime = value; }
		}

		private Version _Version;
		public Version Version
		{
			get { return _Version; }
			set { _Version = value; }
		}

		private Guid _Mutex;
		public Guid Mutex
		{
			get { return _Mutex; }
			set { _Mutex = value; }
		}

		private string _DefaultGroup;
		public string DefaultGroup
		{
			get { return _DefaultGroup; }
			set { _DefaultGroup = value; }
		}

		private string _PrimaryConnectionHost;
		public string PrimaryConnectionHost
		{
			get { return _PrimaryConnectionHost; }
			set { _PrimaryConnectionHost = value; }
		}

		private string _BackupConnectionHost;
		public string BackupConnectionHost
		{
			get { return _BackupConnectionHost; }
			set { _BackupConnectionHost = value; }
		}

		private ushort _ConnectionPort;
		public ushort ConnectionPort
		{
			get { return _ConnectionPort; }
			set { _ConnectionPort = value; }
		}

		private bool _RunOnStartup;
		public bool RunOnStartup
		{
			get { return _RunOnStartup; }
			set { _RunOnStartup = value; }
		}

		private bool _RequestElevation;
		public bool RequestElevation
		{
			get { return _RequestElevation; }
			set { _RequestElevation = value; }
		}

		private bool _BypassUserAccountControl;
		public bool BypassUserAccountControl
		{
			get { return _BypassUserAccountControl; }
			set { _BypassUserAccountControl = value; }
		}

		private byte[] _BypassUserAccountControlData;
		public byte[] BypassUserAccountControlData
		{
			get { return _BypassUserAccountControlData; }
			set { _BypassUserAccountControlData = value; }
		}

		private bool _ClearZoneIdentifier;
		public bool ClearZoneIdentifier
		{
			get { return _ClearZoneIdentifier; }
			set { _ClearZoneIdentifier = value; }
		}

		private bool _ClearAccessControl;
		public bool ClearAccessControl
		{
			get { return _ClearAccessControl; }
			set { _ClearAccessControl = value; }
		}

		private bool _SetCriticalProcess;
		public bool SetCriticalProcess
		{
			get { return _SetCriticalProcess; }
			set { _SetCriticalProcess = value; }
		}

		private bool _PreventSystemSleep;
		public bool PreventSystemSleep
		{
			get { return _PreventSystemSleep; }
			set { _PreventSystemSleep = value; }
		}

		private bool _ActivateAwayMode;
		public bool ActivateAwayMode
		{
			get { return _ActivateAwayMode; }
			set { _ActivateAwayMode = value; }
		}

		private bool _EnableDebugMode;
		public bool EnableDebugMode
		{
			get { return _EnableDebugMode; }
			set { _EnableDebugMode = value; }
		}

		private int _RunDelay;
		public int RunDelay
		{
			get { return _RunDelay; }
			set { _RunDelay = value; }
		}

		private int _ConnectionDelay;
		public int ConnectionDelay
		{
			get { return _ConnectionDelay; }
			set { _ConnectionDelay = value; }
		}

		private int _RestartDelay;
		public int RestartDelay
		{
			get { return _RestartDelay; }
			set { _RestartDelay = value; }
		}

		private int _TimeoutInterval;
		public int TimeoutInterval
		{
			get { return _TimeoutInterval; }
			set { _TimeoutInterval = value; }
		}

		private int _KeepAliveTimeout;
		public int KeepAliveTimeout
		{
			get { return _KeepAliveTimeout; }
			set { _KeepAliveTimeout = value; }
		}

		private int _MutexTimeout;
		public int MutexTimeout
		{
			get { return _MutexTimeout; }
			set { _MutexTimeout = value; }
		}

		private int _LanTimeout;
		public int LanTimeout
		{
			get { return _LanTimeout; }
			set { _LanTimeout = value; }
		}

		private int _WanTimeout;
		public int WanTimeout
		{
			get { return _WanTimeout; }
			set { _WanTimeout = value; }
		}

		private int _BufferSize;
		public int BufferSize
		{
			get { return _BufferSize; }
			set { _BufferSize = value; }
		}

		private int _MaxPacketSize;
		public int MaxPacketSize
		{
			get { return _MaxPacketSize; }
			set { _MaxPacketSize = value; }
		}

		private int _GCThreshold;
		public int GCThreshold
		{
			get { return _GCThreshold; }
			set { _GCThreshold = value; }
		}

		private bool _UseCustomDnsServer;
		public bool UseCustomDnsServer
		{
			get { return _UseCustomDnsServer; }
			set { _UseCustomDnsServer = value; }
		}

		private string _PrimaryDnsServer;
		public string PrimaryDnsServer
		{
			get { return _PrimaryDnsServer; }
			set { _PrimaryDnsServer = value; }
		}

		private string _BackupDnsServer;
		public string BackupDnsServer
		{
			get { return _BackupDnsServer; }
			set { _BackupDnsServer = value; }
		}

		private bool _ShowInstallationDialog;
		public bool ShowInstallationDialog
		{
			get { return _ShowInstallationDialog; }
			set { _ShowInstallationDialog = value; }
		}

		private string _InstallationDialogTitle;
		public string InstallationDialogTitle
		{
			get { return _InstallationDialogTitle; }
			set { _InstallationDialogTitle = value; }
		}

		private string _InstallationDIalogMessage;
		public string InstallationDIalogMessage
		{
			get { return _InstallationDIalogMessage; }
			set { _InstallationDIalogMessage = value; }
		}

		private byte _InstallationDIalogIcon;
		public byte InstallationDIalogIcon
		{
			get { return _InstallationDIalogIcon; }
			set { _InstallationDIalogIcon = value; }
		}
	}
}
