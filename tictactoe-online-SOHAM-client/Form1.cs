using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace tictactoe_online_SOHAM_client
{
    public partial class Form1 : Form
    {
        public IPEndPoint server;
        public byte[] buffer;
        public Socket so;
        public StringBuilder msg;
        public string message;
        public int player,won;
        public Button[] button;


        public void convert()
        {
                for (int i = 0; i < 9; i++)
                this.msg[i] = this.message[i];
            //MessageBox.Show("COMVERSION COMPLETE ");
            
        }

        public void game()
        {
            if (player == 1)
                label2.Text = "WAIT FOR PLAYER 2";
            else
                label2.Text = "WAIT FOR PLAYER 1";
            Application.DoEvents();

            string temp = "090909090";
            while (temp.CompareTo("000000000") != 0)            // 000000000 is sent by server when one player has given their move
            {
                so.Receive(buffer);
                temp = Encoding.ASCII.GetString(buffer);
               // if (temp.Length > 0)
               //     MessageBox.Show("RECIEVED FROM SERVER : " + temp);
                if (temp.CompareTo("555555555") == 0)        // 555555555 is sent by server when Player wins
                {
                    this.won = 1;
                    break;
                }
                if (temp.CompareTo("999999999") == 0)        // 999999999 is sent by server when Player loses
                {
                    this.won = -1;
                    break;
                }

                if (temp.CompareTo("595959595") == 0)
                {
                    this.won = 0;
                    break;
                }
                
            
            
            }

            if (((this.won == -1) || (this.won == 1))||(this.won == 0))
            finish();
            

            else
            {
                so.Receive(buffer);
                message = Encoding.ASCII.GetString(buffer);                  //  Here Server send the current Board Configuration
                for (int i = 0; i < 9; i++)
                    msg[i] = message[i];

                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }

                label2.Text = "NOW GIVE YOUR MOVE";
                Application.DoEvents();
            }
            }



        public void finish()
        {

            so.Receive(buffer);                              // Both players Recieve the winning combination
            message = Encoding.ASCII.GetString(buffer);              
            for (int i = 0; i < 9; i++)
                msg[i] = message[i];

            for (int i = 0; i < 9; i++)
            {
                if (msg[i] == '2')
                    button[i + 1].Text = "" + "X";
                if (msg[i] == '3')
                    button[i + 1].Text = "" + "O";
            }
            
            
            if (this.won == 1)
            {
                label2.Text = "CONGRATULATIONS. YOU WIN ";
                Application.DoEvents();
            }
            if (this.won == -1)
            {
                label2.Text = "SORRY. YOU LOSE ";
                Application.DoEvents();
            }

            if (this.won == 0)
            {
                label2.Text = "It's a DRAW ";
                Application.DoEvents();
            }
            
        }

        
        
        
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("WELCOME TO ONLINE TIC TAC TOE designed by Soham Chakraborty. The first Player to LOGIN to the SERVER Will Play as PLAYER X and will play First.   ENJOY !!! ");
           // server = new IPEndPoint(IPAddress.Parse(textBox1.Text),Int32.Parse(textBox2.Text));
            so = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            label2.Text = "PLEASE ENTER THE GAME SERVER SOCKET INFORMATION and Click CONNECT ";
            Application.DoEvents();
            button = new Button[10];
            button[1] = button1;
            button[2] = button2;
            button[3] = button3;
            button[4] = button4;
            button[5] = button5;
            button[6] = button6;
            button[7] = button7;
            button[8] = button8;
            button[9] = button9;
            this.won = 2;
            msg = new StringBuilder("000000000");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[0] == '1')
            {
                if (player == 1)
                    msg[0] = '2';
                else
                    msg[0] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
              //  MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            server = new IPEndPoint(IPAddress.Parse(textBox1.Text), Int32.Parse(textBox2.Text));
            so.Connect(server);
            buffer = new byte[9];
            so.Receive(buffer);
            message = Encoding.ASCII.GetString(buffer);
            if (message.CompareTo("000000000")==0)   // 000000000 sent to player 1 by server
            {
                MessageBox.Show("CONNECTED TO SERVER. Please Wait for Player 2 to Join");
                label3.Text = "YOU PLAY AS PLAYER X";
                player = 1;
                label2.Text = "PLEASE WAIT FOR PLAYER 2";
                Application.DoEvents();
            }

            if (message.CompareTo("999999999")==0)                            // 999999999 sent to player 2 by server
            {
                MessageBox.Show("CONNECTED TO SERVER. ");
                label3.Text = "YOU PLAY AS PLAYER O";
                player = 2;
                Application.DoEvents();
            }

            if (player == 1)
            {
                while (true)
                {
                    so.Receive(buffer);
                    message = Encoding.ASCII.GetString(buffer);
                    if (message.CompareTo("111111111") == 0)          // 111111111 is sent by server when both player have joined
                        break;
                }
              //  MessageBox.Show("GAME STARTED");
                label2.Text = "GIVE YOUR MOVE";
                
                Application.DoEvents();
                
                
            
            }

            if (player == 2)
            {
                MessageBox.Show("GAME STARTED");
                game();
            }
        
            }

        private void button2_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[1] == '1')
            {
                if (player == 1)
                    msg[1] = '2';
                else
                    msg[1] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
              //  MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[2] == '1')
            {
                if (player == 1)
                    msg[2] = '2';
                else
                    msg[2] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
              //  MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[3] == '1')
            {
                if (player == 1)
                    msg[3] = '2';
                else
                    msg[3] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
             //   MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void button5_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[4] == '1')
            {
                if (player == 1)
                    msg[4] = '2';
                else
                    msg[4] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
             //   MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void button6_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[5] == '1')
            {
                if (player == 1)
                    msg[5] = '2';
                else
                    msg[5] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
             //   MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void button7_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[6] == '1')
            {
                if (player == 1)
                    msg[6] = '2';
                else
                    msg[6] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
             //   MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void button8_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[7] == '1')
            {
                if (player == 1)
                    msg[7] = '2';
                else
                    msg[7] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
             //   MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }

        private void button9_Click(object sender, EventArgs e)
        {
            convert();
            if (msg[8] == '1')
            {
                if (player == 1)
                    msg[8] = '2';
                else
                    msg[8] = '3';
                for (int i = 0; i < 9; i++)
                {
                    if (msg[i] == '2')
                        button[i + 1].Text = "" + "X";
                    if (msg[i] == '3')
                        button[i + 1].Text = "" + "O";
                }
                Application.DoEvents();
          //      MessageBox.Show("THE SENDING STRING IS " + msg.ToString());
                so.Send(Encoding.ASCII.GetBytes(msg.ToString()));
                game();
            }
        
        }
    }
}