using System;
using System.Text;
using System.Windows.Forms;
using System.Net;

using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Net.Sockets;


namespace web_word
{
    public partial class Form1 : Form
    {
        TcpClient tc;
        NetworkStream stream;
        public Form1()
        {
            InitializeComponent();
            // 디폴트값 사용 (Maximum=100, Minimum=0, Step=10)
            progressBar1.Style = ProgressBarStyle.Blocks;
            //tc = new TcpClient("127.0.0.1", 9999);
           // stream = tc.GetStream();
            
            
        }
        private void Sreach_button_Click(object sender, EventArgs e)
        {

            
            listView1.HotTracking = true;
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("검색창을 작성하세요!");
            }
            else
            {
                String[] texts = new String[1];
                String[] arr = new String[3];
                string query = textBox1.Text; // 검색할 문자열
                string url = "https://openapi.naver.com/v1/search/blog?query=" + query; // 결과가 JSON 포맷
                                                                                        // string url = "https://openapi.naver.com/v1/search/blog.xml?query=" + query;  // 결과가 XML 포맷
               
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Headers.Add("X-Naver-Client-Id", "epyY5bXZb2npJsGQ3p1u"); // 클라이언트 아이디
                    request.Headers.Add("X-Naver-Client-Secret", "Lzwj3w_ZWo");       // 클라이언트 시크릿
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string status = response.StatusCode.ToString();
              
               
                
                if (status == "OK")
                {
                    Stream steam = response.GetResponseStream();
                    StreamReader reader = new StreamReader(steam, Encoding.UTF8);
                    string text = reader.ReadToEnd();
                    Console.WriteLine(text);
                    JObject o = JObject.Parse(text);
                    JArray ant = (JArray)o["items"];
                    progressBar1.Value = 0;
                    String[] kok = new string[1];
                    kok[0] = text;
                    ListViewItem it2 = new ListViewItem(kok);
                    listView2.Items.Add(textBox1.Text);
                    
                    //byte[] buff = Encoding.UTF8.GetBytes(text.ToString());
                    //stream.Write(buff,0,buff.Length);
                    for (int i = 0; i < ant.Count; i++)
                    {
                        tc = new TcpClient("localhost", 7000);
                        stream = tc.GetStream();

                        var json = new JObject();
                        JObject j = (JObject)ant[i];
                        string title = (string)j["title"];
                        title = Regex.Replace(title, "(<([^>]+)>)", "");
                        string description = (string)j["description"];
                        description = Regex.Replace(description, "(<([^>]+)>)", "");
                        string link = (string)j["link"];
                        link = Regex.Replace(link, "(<([^>]+)>)", "");
                        link = "<a href=" + link + "link</a>";
                        json.Add("title", title);
                        json.Add("description", description);

                        byte[] buff1 = Encoding.UTF8.GetBytes(json.ToString());
              
                        //검색어 집어 넣기
                        arr[0] = title;
                        arr[1] = description;
                        arr[2] = link;
                        ListViewItem it = new ListViewItem(arr);
                        listView1.Items.Add(it);
                        progressBar1.PerformStep();
                        string str1 = Encoding.UTF8.GetString(buff1);                      
                        

                        stream.Write(buff1, 0, buff1.Length);
                        //Console.WriteLine("송신: {0}", "buff");
                        stream.Close();
                        
                    }
                    tc.Close();
                }
                else
                {
                    MessageBox.Show("Error 발생=" + status);
                }   
            }
        }
        


        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Add("제목", 246);
            listView1.Columns.Add("내용", 246);
            listView1.Columns.Add("주소", 246);
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView2.CheckBoxes = true;
            listView2.View = View.Details;
            listView2.Columns.Add("검색기록", 116);
            listView2.GridLines = true;
            listView2.FullRowSelect = true;
            listView3.View = View.Details;
            listView3.Columns.Add("검색기록", 116);
            listView3.GridLines = true;
            listView3.FullRowSelect = true;
            listView4.View = View.Details;
            listView4.Columns.Add("제목", 350);
            listView4.Columns.Add("내용", 350);
            //listView4.Columns.Add("", );
            listView4.GridLines = true;
            listView4.FullRowSelect = true;

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}

