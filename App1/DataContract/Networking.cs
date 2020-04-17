//----------------------------------------------------------------------------------------------
// <copyright file="Networking.cs" company="Microsoft Corporation">
//     Licensed under the MIT License. See LICENSE.TXT in the project root license information.
// </copyright>
// MODIFIED By RR
//----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SynergizDiag
{
    /// <content>
    /// Wrappers for Networking methods.
    /// </content>
    /// RR : Removed the connection part, keeped the DataContract part and added ToString Substitutions
    public partial class DevicePortal
    {
        #region Data contract

        /// <summary>
        /// DHCP object.
        /// </summary>
        [DataContract]
        public class Dhcp
        {
            /// <summary>
            ///  Gets the time at which the lease will expire, in ticks.
            /// </summary>
            [DataMember(Name = "LeaseExpires")]
            public long LeaseExpiresRaw { get; private set; }

            /// <summary>
            /// Gets the time at which the lease was obtained, in ticks.
            /// </summary>
            [DataMember(Name = "LeaseObtained")]
            public long LeaseObtainedRaw { get; private set; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            [DataMember(Name = "Address")]
            public IpAddressInfo Address { get; private set; }

            /// <summary>
            /// Gets the lease expiration time.
            /// </summary>
            public DateTimeOffset LeaseExpires
            {
                get { return new DateTimeOffset(new DateTime(this.LeaseExpiresRaw)); }
            }

            /// <summary>
            /// Gets the lease obtained time.
            /// </summary>
            public DateTimeOffset LeaseObtained
            {
                get { return new DateTimeOffset(new DateTime(this.LeaseObtainedRaw)); }
            }

            public override string ToString()
            {
                return Address.ToString();

            }
        }

        /// <summary>
        /// IP Address info
        /// </summary>
        [DataContract]
        public class IpAddressInfo
        {
            /// <summary>
            /// Gets the address
            /// </summary>
            [DataMember(Name = "IpAddress")]
            public string Address { get; private set; }

            /// <summary>
            /// Gets the subnet mask
            /// </summary>
            [DataMember(Name = "Mask")]
            public string SubnetMask { get; private set; }

            public override string ToString()
            {
                return new StringBuilder(Address).Append(" : ").Append(SubnetMask).ToString();

            }

        }

        /// <summary>
        /// IP Configuration object
        /// </summary>
        [DataContract]
        public class IpConfiguration
        {
            /// <summary>
            /// Gets the list of networking adapters
            /// </summary>
            [DataMember(Name = "Adapters")]
            public List<NetworkAdapterInfo> Adapters { get; private set; }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                foreach (NetworkAdapterInfo item in Adapters)
                {
                    sb.AppendLine(item.ToString());
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Networking adapter info
        /// </summary>
        [DataContract]
        public class NetworkAdapterInfo
        {
            /// <summary>
            /// Gets the description
            /// </summary>
            [DataMember(Name = "Description")]
            public string Description { get; private set; }

            /// <summary>
            /// Gets the hardware address
            /// </summary>
            [DataMember(Name = "HardwareAddress")]
            public string MacAddress { get; private set; }

            /// <summary>
            /// Gets the index
            /// </summary>
            [DataMember(Name = "Index")]
            public int Index { get; private set; }

            /// <summary>
            /// Gets the name
            /// </summary>
            [DataMember(Name = "Name")]
            public Guid Id { get; private set; }

            /// <summary>
            /// Gets the type
            /// </summary>
            [DataMember(Name = "Type")]
            public string AdapterType { get; private set; }

            /// <summary>
            /// Gets DHCP info
            /// </summary>
            [DataMember(Name = "DHCP")]
            public Dhcp Dhcp { get; private set; }

            // TODO - WINS

            /// <summary>
            /// Gets Gateway info
            /// </summary>
            [DataMember(Name = "Gateways")]
            public List<IpAddressInfo> Gateways { get; private set; }

            /// <summary>
            /// Gets the list of IP addresses
            /// </summary>
            [DataMember(Name = "IpAddresses")]
            public List<IpAddressInfo> IpAddresses { get; private set; }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Description : ").AppendLine(Description);
                sb.Append("MacAddress : ").AppendLine(MacAddress);
                sb.Append("Index : ").AppendLine(Index.ToString());
                sb.Append("Id : ").AppendLine(Id.ToString());
                sb.Append("Type : ").AppendLine(AdapterType);
                sb.Append("DHCP : ").AppendLine(Dhcp.ToString());
                sb.AppendLine("Gateways :");
                foreach (IpAddressInfo item in Gateways)
                {
                    sb.AppendLine(item.ToString());
                }
                sb.AppendLine("IpAdresses :");
                foreach (IpAddressInfo item in IpAddresses)
                {
                    sb.AppendLine(item.ToString());
                }
                return sb.ToString();
            }

            

        }
        #endregion // Data contract
    }
}
