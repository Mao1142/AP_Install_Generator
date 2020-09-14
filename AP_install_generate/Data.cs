using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_install_generate
{
    class Data
    {
        private string IP { get; set; }
        private string SSID{ get; set; }
        private string PW { get; set; }
        public Data(string ip,string ssid, string pw)
        {
            IP = ip;
            SSID = ssid;
            PW = pw;
        }
        public string script_generate()
        {
            string[] splIP = IP.Split('.');
            string mIP = $"{splIP[0]}.{splIP[1]}.{splIP[2]}";
            string content =
                "echo Install Start\n"+
                "echo Install dnsmasq and hostapd\n" +
                "sudo apt-get --yes install dnsmasq hostapd\n\n" +
                //dhcpcd.conf
                "echo Setting DHCP\n" +
                "sudo cat>>/etc/dhcpcd.conf<<EOF\n" +
                "#DISABLE WLAN0 FROM DHCPCD AS WE ARE USING IT AS A WIFI ACCESS POINT\n" +
                "interface wlan0\n" +
                $"\tstatic ip_address={IP}/24\n" +
                "\tnohook wpa_supplicant\n" +
                "EOF\n\n"+
                //interface
                "echo Setting staticIP\n" +
                "sudo cat>>/etc/network/interfaces<<EOF\n" +
                "allow-hotplug wlan0\n" +
                "# iface wlan0 inet static\n" +
                "wireless - mode Master\n"+
                $"address {IP}\n"+
                "netmask 255.255.255.0\n"+
                $"network {mIP}.0\n"+
                $"broadcast {mIP}.255\n"+
                "#wpa-conf /etc/wpa_supplicant/wpa_supplicant.conf\n"+
                "EOF\n\n"+
                //hostapd.config
                "echo Setting AP info\n" +
                "sudo cat>>/etc/hostapd/hostapd.conf<<EOF\n" +
                "interface=wlan0\n" +
                "\n# Use the nl80211 driver with the brcmfmac driver\n" +
                "driver = nl80211\n" +
                "\n# The name to use for the network\n" +
                $"ssid = {SSID}\n" +
                "\n# Use the 2.4GHz band\n" +
                "hw_mode = g\n" +
                "\n# Use channel 6\n" +
                "channel = 6\n" +
                "\n# Enable 802.11n\n" +
                "ieee80211n = 1\n" +
                "\n# Enable WMM\n" +
                "wmm_enabled = 1\n" +
                "\n# Enable 40MHz channels with 20ns guard interval\n" +
                "ht_capab =[HT40][SHORT - GI - 20][DSSS_CCK - 40]\n" +
                "\n# Accept all MAC addresses\n" +
                "macaddr_acl = 0\n" +
                "\n# Use WPA authentication\n" +
                "auth_algs = 1\n" +
                "\n# Broadcast the network name\n" +
                "ignore_broadcast_ssid = 0\n" +
                "\n# Use WPA2\n" +
                "wpa = 2\n" +
                "\n# Use a pre-shared key\n" +
                "wpa_key_mgmt = WPA - PSK\n" +
                "\n# The WPA2 passphrase (password)\n" +
                $"wpa_passphrase = {PW}\n" +
                "\n# Use AES, instead of TKIP\n" +
                "rsn_pairwise = CCMP\n"+
                "EOF\n\n" +
                //hostapd
                "echo Setting Configuration file\n" +
                "sudo cat>>/home/pi/hostapd<<EOF\n" +
                $"{Properties.Resources.hostapd}\n"+
                "EOF\n\n"+
                "sudo cp /home/pi/hostapd /etc/default/\n" +
                "sudo rm -f /home/pi/hostapd\n" +
                //dnsmasq.conf
                "echo Setting DNSMASQ\n" +
                "sudo mv /etc/dnsmasq.conf /etc/dnsmasq.conf.orig\n" +
                "sudo cat>>/etc/dnsmasq.conf<<EOF\n" +
                "interface=wlan0 # Use the require wireless interface – usually wlan0\n" +
                $"dhcp-range={mIP}.2,{mIP}.200,255.255.255.0,24h\n" +
                "EOF\n\n"+
                //sysctl.conf
                "echo Setting IP4 forward\n" +
                "sudo cat>>/home/pi/sysctl.conf<<EOF\n" +
                $"{Properties.Resources.sysctl}\n" +
                "EOF\n\n"+
                "sudo cp /home/pi/sysctl.conf /etc/\n" +
                "sudo rm -f /home/pi/sysctl.conf \n" +
                //start service
                @"sudo sh - c ""echo 1 > /proc/sys/net/ipv4/ip_forward"""+"\n"+
                //connect wlan0 & eth0
                "sudo iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE\n" +
                "sudo iptables -A FORWARD -i eth0 -o wlan0 -m state --state RELATED,ESTABLISHED -j ACCEPT\n" +
                "sudo iptables -A FORWARD -i wlan0 -o eth0 -j ACCEPT\n" +
                @"sudo sh -c ""iptables - save > / etc / iptables.ipv4.nat""" + "\n" +
                //rc.local
                "sudo cat>>/home/pi/rc.local<<EOF\n" +
                $"{Properties.Resources.rc_local}\n"+
                "EOF\n"+
                "sudo cp /home/pi/rc.local /etc/\n" +
                "sudo rm -f /home/pi/rc.local\n" +
                //Enable service
                "sudo systemctl unmask hostapd\n" +
                "sudo service hostapd start\n" +
                "sudo service dnsmasq start\n";
            return content;
        }
        





    }
}
