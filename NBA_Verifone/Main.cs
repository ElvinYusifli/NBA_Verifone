using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Main : Form
    {
        InTheHand.Net.Sockets.BluetoothClient client = null;
        NetworkStream stream = null;

        public Main()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label3.Text = "Status: Disconnect";
        }


        private async void button1_Click(object sender, EventArgs e)
        {

            byte[] write = System.Text.Encoding.ASCII.GetBytes(textBox1.Text);
            stream.Write(write, 0, write.Length);

            Thread.Sleep(1000);

            if (stream.DataAvailable)
            {
                byte[] received = new byte[stream.Length];
                stream.Read(received, 0, received.Length);
                string receivedString = Encoding.ASCII.GetString(received);
                textBox2.Text += receivedString+ Environment.NewLine;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            BluetoothDevicePicker picker = new BluetoothDevicePicker();

            var device = await picker.PickSingleDeviceAsync();


            listBox1.Items.Clear();
            listBox2.Items.Clear();

            client = new InTheHand.Net.Sockets.BluetoothClient();
            client.Connect(device.DeviceAddress, new Guid("fa87c0d0-afac-11de-8a39-0800200c9a66"));
            stream = client.GetStream();
            label3.Text = "Status: Connected " + device.DeviceName;

            //var AvilableDevices = client.DiscoverDevices();
            //foreach (var item in AvilableDevices)
            //{
            //    listBox1.Items.Add(item.DeviceName);
            //}



            //var PairDevices = client.PairedDevices;
            //foreach (var item in PairDevices)
            //{
            //    listBox2.Items.Add(item.DeviceName);
            //}

            btnConnect.Enabled = !client.Connected;
            btnDisconnect.Enabled = client.Connected;
            btnSend.Enabled = client.Connected;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (client.Connected)
            {
                client.Close();
                stream.Close();
                client.Dispose();
                stream.Dispose();
                label3.Text = "Status: Disconnect";
                btnConnect.Enabled = !client.Connected;
                btnDisconnect.Enabled = client.Connected;
                btnSend.Enabled = client.Connected;
            }

        }
    }
}
