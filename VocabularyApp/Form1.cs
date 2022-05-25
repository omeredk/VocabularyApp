using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VocabularyApp
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        
        //YOU SHOULD ENTER YOUR CONNECTION STRING FIRST
        SqlConnection connection = new SqlConnection("Data Source=DESKTOP-CJTBSKD;Initial Catalog=dictionaryDB;Integrated Security=True");

        //FIND FUNCTION STARTS HERE
        void listWord()
        {
            connection.Open();
            SqlCommand cmdList = new SqlCommand("SELECT translation FROM words WHERE word='"+textBoxWord.Text+"'", connection);
            SqlDataReader dr = cmdList.ExecuteReader();
            if (dr.Read())
            {
                richTextBoxMeaning.Text = dr["translation"].ToString();
                dr.Close();
            }
            else
            {
                MessageBox.Show("Word Not Found", "ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                dr.Close();
            }
            connection.Close();
        }

        //ADD FUNCTION STARTS HERE
        private void btnAdd_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand cmdCheck = new SqlCommand("SELECT * FROM words WHERE word='" + textBoxWord.Text + "'", connection);
            SqlDataReader dr = cmdCheck.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("entered word is already exists! ", "WARNING");
                dr.Close();
            }

            else
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO words(word, translation) VALUES(@1,@2)", connection);
                cmd.Parameters.AddWithValue("@1", textBoxWord.Text);
                cmd.Parameters.AddWithValue("@2", richTextBoxMeaning.Text);
                dr.Close();
                cmd.ExecuteNonQuery();
                MessageBox.Show("SUCCES!");
            }
                connection.Close();
            textBoxWord.Text = "";
            richTextBoxMeaning.Text = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            listWord();
        }

        //DELETE FUNCTION STARTS HERE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult answer;
            answer = MessageBox.Show("Are you sure you want to delete this?", "WARNING!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (answer == DialogResult.Yes)
            {
                connection.Open();
                SqlCommand cmdDelete = new SqlCommand("DELETE FROM words WHERE word='" + textBoxWord.Text + "'", connection);
                cmdDelete.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Word has been deleted", "Succes!");
            }
            textBoxWord.Text = "";
            richTextBoxMeaning.Text = "";
        }
    }
}
