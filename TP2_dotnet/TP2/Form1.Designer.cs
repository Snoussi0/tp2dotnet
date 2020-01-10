using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace TP2
{
    
    
        public partial class Form1 : Form
        {
            private SqlConnection cn = new SqlConnection("Data Source=ASUS/PC-BAHA; initial catalog=Clients; integrated security=true");


            private bool rechercher_cin(string s) //fonction qui permet de vérifier l'existence d'un client
            {
                bool p = false;
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "select * from client where cin='" + s + "'";
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    p = true;

                dr.Close();
                cn.Close();
                return p;
            }

            private void actualiser()   //procédure qui permet de charger le contenu de la table client dans le Datagridview
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("select * from client", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dr.Close();
                cn.Close();
            }




            public Form1()
            {
                InitializeComponent();
            }

            private void Form1_Load(object sender, EventArgs e)
            {

                actualiser(); //charger le contenu de la table client dans le datagridwiew au démarrage du formulaire
            }

            private void btn_nouveau_Click(object sender, EventArgs e) //vider les champs du formulaire
            {
                txt_cin.Text = "";
                txt_nom.Text = "";
                txt_prenom.Text = "";
                txt_ville.Text = "";
                txt_tel.Text = "";
            }

            private void btn_rechercher_Click(object sender, EventArgs e)
            {
                if (txt_cin.Text == "") //controle de saisie sur le champ CIN
                    MessageBox.Show("Champ CIN vide!!", "champ vide", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (rechercher_cin(txt_cin.Text) == false) // vérification de l'existence du client dont le CIN est saisi dans la zone CIN
                {
                    txt_nom.Text = ""; // vider les valeurs des recherches précédentes
                    txt_prenom.Text = "";
                    txt_ville.Text = "";
                    txt_tel.Text = "";
                    MessageBox.Show("Client introuvable", "introuvable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_cin.Focus();
                    txt_cin.SelectAll(); //selectionner le champ CIN pour une éventuelle resaisie
                }
                else
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "select * from client where cin='" + txt_cin.Text + "'";
                    SqlDataReader dr = cmd.ExecuteReader();
                    dr.Read(); // un seul client 
                    txt_nom.Text = dr[1].ToString();
                    txt_prenom.Text = dr[2].ToString();
                    txt_ville.Text = dr[3].ToString();
                    txt_tel.Text = dr[4].ToString();
                    dr.Close();
                    cn.Close();

                }
            }

            private void btn_ajouter_Click(object sender, EventArgs e)
            {
                if (txt_cin.Text == "" || txt_nom.Text == "" || txt_prenom.Text == "" || txt_ville.Text == "" || txt_tel.Text == "") //controle de saisie
                {
                    MessageBox.Show("Champ vide!!", "champ vide", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (rechercher_cin(txt_cin.Text) == true) // vérification de l'unicité du client dont le CIN est saisi dans la zone CIN
                {

                    MessageBox.Show("Client existe déja", "existant", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_cin.Focus();
                    txt_cin.SelectAll();
                }
                else
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = String.Format("insert into client values('{0}','{1}','{2}','{3}','{4}')", txt_cin.Text, txt_nom.Text, txt_prenom.Text, txt_ville.Text, txt_tel.Text);
                    int r = cmd.ExecuteNonQuery(); // ou ecrire cmd.ExecuteNonQuery() sans retour
                    if (r != 0)
                    {
                        MessageBox.Show("Client bien ajouté", "ajout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close(); // fermer la connexion avant d'appeler la méthode actualiser
                        actualiser(); //charger à nouveau les données pour afficher le nouveau client
                    }

                }

            }

            private void btn_modifier_Click(object sender, EventArgs e)
            {
                if (txt_cin.Text == "" || txt_nom.Text == "" || txt_prenom.Text == "" || txt_ville.Text == "" || txt_tel.Text == "") //controle de saisie
                {
                    MessageBox.Show("Champ vide!!", "champ vide", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (rechercher_cin(txt_cin.Text) == false) // vérification de l'existence du client dont le CIN est saisi dans la zone CIN
                {

                    MessageBox.Show("Client Introuvable", "Introuvable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_cin.Focus();
                    txt_cin.SelectAll(); //selectionner le champ CIN pour une éventuelle resaisie
                }
                else
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = String.Format("update client set nom='{0}',prenom='{1}',ville='{2}',tel='{3}' where cin='{4}'", txt_nom.Text, txt_prenom.Text, txt_ville.Text, txt_tel.Text, txt_cin.Text);
                    int r = cmd.ExecuteNonQuery();
                    if (r != 0)
                    {
                        MessageBox.Show("Client bien Modifié", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close();
                        actualiser();
                    }

                }
            }

            private void btn_supprimer_Click(object sender, EventArgs e)
            {
                if (txt_cin.Text == "") //controle de saisie
                {
                    MessageBox.Show("Champ CIN vide!!", "champ vide", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (rechercher_cin(txt_cin.Text) == false) // vérification de l'existence du client dont le CIN est saisi dans la zone CIN
                {

                    MessageBox.Show("Client Introuvable", "Introuvable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_cin.Focus();
                    txt_cin.SelectAll(); //selectionner le champ CIN pour une éventuelle resaisie
                }
                else
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = String.Format("delete from client where cin='{0}'", txt_cin.Text);
                    int r = cmd.ExecuteNonQuery();
                    if (r != 0)
                    {
                        MessageBox.Show("Client bien Supprimé", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btn_nouveau.PerformClick(); //appeler le bouton nouveau pour vider les champs
                        cn.Close();
                        actualiser();
                    }

                }
            }

            private void btn_quitter_Click(object sender, EventArgs e)
            {
                this.Close();
            }
        }
    }
 
