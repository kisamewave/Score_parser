using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace WindowsFormsApplication10
{
    public class NameAndScore
    {
        public string Name { get; set; }
        public string Score { get; set; }

    }
    public partial class Form1 : Form
    {
        DataTable table;
        HtmlWeb web = new HtmlWeb();
        public Form1()
        {
            InitializeComponent();
            InitTable();
        }
        private void InitTable()
        {
            table = new DataTable("GameRankingDataTable");
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Score", typeof(string));
            //table.Rows.Add("papa", "dd");
            gameRankingDataView.DataSource = table;
        }
        private async Task<List<NameAndScore>> GameRangingFromPage(int pageNum) {
            string url= "http://www.gamerankings.com/browse.html";
            if (pageNum != 0)
                 url = "http://www.gamerankings.com/browse.html?page=" + pageNum.ToString();
            var doc = await Task.Factory.StartNew(() => web.Load(url));

            var nameNodes = doc.DocumentNode.SelectNodes("//*[@id=\"main_col\"]//div//div//table///tr//td//a");
            var scoreNodes = doc.DocumentNode.SelectNodes("//*[@id=\"main_col\"]//div//div//table//tbody//tr//td//span//b");
            if (nameNodes == null || scoreNodes == null)
                return new List<NameAndScore>();
            var names = nameNodes.Select(node => node.InnerText);
            var scores = nameNodes.Select(node => node.InnerText);
            return names.Zip(scores, (name, score) => new NameAndScore() { Name = name, Score = score }).ToList();
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            
            var ranking =  await GameRangingFromPage(0);
            foreach (var i in ranking)
                table.Rows.Add(i.Name, i.Score);
        }

        
        
    }
}
