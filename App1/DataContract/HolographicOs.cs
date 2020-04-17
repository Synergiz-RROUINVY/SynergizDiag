// <copyright file="HolographicOs.cs" company="Microsoft Corporation">
//     Licensed under the MIT License. See LICENSE.TXT in the project root license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace Microsoft.Tools.WindowsDevicePortal
{
    /// <content>
    /// Wrappers for Holographic OS methods
    /// </content>
    public partial class DevicePortal
    {
        /// <summary>
        /// API for getting or setting Interpupilary distance
        /// </summary>
        public static readonly string HolographicIpdApi = "api/holographic/os/settings/ipd";

        /// <summary>
        /// API for getting a list of running HoloLens specific services.
        /// </summary>
        public static readonly string HolographicServicesApi = "api/holographic/os/services";

        /// <summary>
        /// API for getting or setting HTTPS setting
        /// </summary>
        public static readonly string HolographicWebManagementHttpSettingsApi = "api/holographic/os/webmanagement/settings/https";

        /// <summary>
        /// Enumeration describing the status of a process
        /// </summary>
        public enum ProcessStatus
        {
            /// <summary>
            /// The process is running
            /// </summary>
            Running = 0,
            
            /// <summary>
            /// The process is stopped
            /// </summary>
            Stopped
        }


        #region Data contract
        /// <summary>
        /// Object reporesentation of the status of the Holographic services
        /// </summary>
        [DataContract]
        public class HolographicServices
        {
            /// <summary>
            /// Gets the status for the collection of holographic services
            /// </summary>
            [DataMember(Name = "SoftwareStatus")]
            public HolographicSoftwareStatus Status { get; private set; }
        }

        /// <summary>
        /// Object representation of the collection of holographic services.
        /// </summary>
        [DataContract]
        public class HolographicSoftwareStatus
        {
            /// <summary>
            /// Gets the status of dwm.exe
            /// </summary>
            [DataMember(Name = "dwm.exe")]
            public ServiceStatus Dwm { get; private set; }

            /// <summary>
            /// Gets the status of holoshellapp.exe
            /// </summary>
            [DataMember(Name = "holoshellapp.exe")]
            public ServiceStatus HoloShellApp { get; private set; }

            /// <summary>
            /// Gets the status of holosi.exe
            /// </summary>
            [DataMember(Name = "holosi.exe")]
            public ServiceStatus HoloSi { get; private set; }

            /// <summary>
            /// Gets the status of mixedrealitycapture.exe
            /// </summary>
            [DataMember(Name = "mixedrealitycapture.exe")]
            public ServiceStatus MixedRealitytCapture { get; private set; }

            /// <summary>
            /// Gets the status of sihost.exe
            /// </summary>
            [DataMember(Name = "sihost.exe")]
            public ServiceStatus SiHost { get; private set; }

            /// <summary>
            /// Gets the status of spectrum.exe
            /// </summary>
            [DataMember(Name = "spectrum.exe")]
            public ServiceStatus Spectrum { get; private set; }
        }

        /// <summary>
        /// Object representation for Interpupilary distance
        /// </summary>
        [DataContract]
        public class InterPupilaryDistance
        {
            /// <summary>
            /// Gets the raw interpupilary distance
            /// </summary>
            [DataMember(Name = "ipd")]
            public int IpdRaw { get; private set; }

            /// <summary>
            /// Gets or sets the interpupilary distance
            /// </summary>
            public float Ipd
            {
                get { return this.IpdRaw / 1000.0f; }
                set { this.IpdRaw = (int)(value * 1000); }
            }
        }

        /// <summary>
        /// Object representation of the status of a  service
        /// </summary>
        [DataContract]
        public class ServiceStatus
        {
            /// <summary>
            /// Gets the raw value returned for the expected service status
            /// </summary>
            [DataMember(Name = "Expected")]
            public string ExpectedRaw { get; private set; }

            /// <summary>
            /// Gets the raw value returned for the observed service status
            /// </summary>
            [DataMember(Name = "Observed")]
            public string ObservedRaw { get; private set; }

            /// <summary>
            /// Gets the the expected service status
            /// </summary>
            public ProcessStatus Expected
            {
                get
                {
                    return (this.ExpectedRaw == "Running") ? ProcessStatus.Running : ProcessStatus.Stopped;
                }
            }

            /// <summary>
            /// Gets the the observed service status
            /// </summary>
            public ProcessStatus Observed
            {
                get
                {
                    return (this.ObservedRaw == "Running") ? ProcessStatus.Running : ProcessStatus.Stopped;
                }
            }
        }

        /// <summary>
        /// Object representation for HTTP settings
        /// </summary>
        [DataContract]
        public class WebManagementHttpSettings
        {
            /// <summary>
            /// Gets a value indicating whether HTTPS is required
            /// </summary>
            [DataMember(Name = "httpsRequired")]
            public bool IsHttpsRequired { get; private set; }
        }

        #endregion // Data contract
    }
}
