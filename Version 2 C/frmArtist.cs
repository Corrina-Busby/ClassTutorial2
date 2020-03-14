using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// feature: more than one artist form at the same time
/// </summary>
namespace Version_2_C
{
    public partial class frmArtist : Form
    {
        public frmArtist()
        {
            InitializeComponent();
        }

        private clsArtist _Artist;
        private clsWorksList _WorksList;

        // clsArtist is the key, frmArtist is the value that we want instances to be stored maybe...?
        private static Dictionary<clsArtist, frmArtist> _ArtistFormList = new Dictionary<clsArtist, frmArtist>();
        
        public static void Run(clsArtist prArtist)
        {
            frmArtist lcArtistForm;
            if (!_ArtistFormList.TryGetValue(prArtist, out lcArtistForm))
            {
                lcArtistForm = new frmArtist();
                _ArtistFormList.Add(prArtist, lcArtistForm);
                lcArtistForm.SetDetails(prArtist);
            }
            else
            {
                lcArtistForm.Show();
                lcArtistForm.Activate();
            }
        }
        private void updateDisplay()
        {
            txtName.Enabled = txtName.Text == "";
            if (_WorksList.SortOrder == 0)
            {
                _WorksList.SortByName();
                rbByName.Checked = true;
            }
            else
            {
                _WorksList.SortByDate();
                rbByDate.Checked = true;
            }

            lstWorks.DataSource = null;
            lstWorks.DataSource = _WorksList;
            lblTotal.Text = Convert.ToString(_WorksList.GetTotalValue());
        }

        private void updateTitle(string prGalleryName)
        {
            if (!string.IsNullOrEmpty(prGalleryName))
                Text = "Artist Details - " + prGalleryName;
        }

        public void SetDetails(clsArtist prArtist)
        {
            _Artist = prArtist;
            txtName.Enabled = string.IsNullOrEmpty(_Artist.Name);
            updateForm();
            updateDisplay();
            Show();
            frmMain.Instance.GalleryNameChanged += new frmMain.Notify(updateTitle);
            updateTitle(_Artist.ArtistList.GalleryName);
        }

        private void updateForm()
        {
            txtName.Text = _Artist.Name;
            txtSpeciality.Text = _Artist.Speciality;
            txtPhone.Text = _Artist.Phone;
            _WorksList = _Artist.WorksList;
        }

        private void pushData()
        {
            _Artist.Name = txtName.Text;
            _Artist.Speciality = txtSpeciality.Text;
            _Artist.Phone = txtPhone.Text;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int lcIndex = lstWorks.SelectedIndex;

            if (lcIndex >= 0 && MessageBox.Show("Are you sure?", "Deleting work", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _WorksList.RemoveAt(lcIndex);
                frmMain.Instance.updateDisplay();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string lcReply = new InputBox(clsWork.FACTORY_PROMPT).Answer;
            if (!string.IsNullOrEmpty(lcReply))
            {
                _WorksList.AddWork(lcReply[0]);
                frmMain.Instance.updateDisplay();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        { //frm close() to hide() for the special feature to work(open multiple frmArtist forms)
            if (isValid() == true)
                try
                {
                    pushData();
                    if (txtName.Enabled)
                    {
                        _Artist.NewArtist();
                        MessageBox.Show("Artist added!", "Success");
                        frmMain.Instance.updateDisplay();
                        txtName.Enabled = false;
                    }
                    Hide();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private Boolean isValid()
        {
            if (txtName.Enabled && txtName.Text != "")
                if (_Artist.IsDuplicate(txtName.Text))
                {
                    MessageBox.Show("Artist with that name already exists!", "Error adding artist");
                    return false;
                }
                else
                    return true;
            else
                return true;
        }

        private void lstWorks_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                _WorksList.EditWork(lstWorks.SelectedIndex);
                updateDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rbByDate_CheckedChanged(object sender, EventArgs e)
        {
            _WorksList.SortOrder = Convert.ToByte(rbByDate.Checked);
            updateDisplay();
        }
    }
}