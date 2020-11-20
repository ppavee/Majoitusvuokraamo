using MajoitusVuokraamo.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MajoitusVuokraamo.Entities;
using MajoitusVuokraamoLib.Entities;

namespace MajoitusVuokraamo
{
    public partial class Form1 : Form
    {
        private Kayttaja nykyinenKayttaja;
        public Form1()
        {
            InitializeComponent();

            // Show initial panels
            loginRegPanel.Visible = true;
            tabControl.Visible = true;

            // Hide others
            logInPanel.Visible = false;
            userMngPanel.Visible = false;
            teeVarausPanel.Visible = false;
            userManagementPanel.Visible = false;

            commonInfoLbl.Text = $"Tänään on {DateTime.Now.ToShortDateString()}";


        }

        #region TOP_PANEL
        #endregion

        #region MAJOITUSKOHTEENI
        private void majoituskohteeniRemoveBtn_Click(object sender, EventArgs e)
        {
            if (majoituskohteeniDataGW.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = majoituskohteeniDataGW.SelectedRows[0];
                int majoitusId = (int)selectedRow.Cells[0].Value;
                Majoitus majoitus = MajoitusController.haeMajoitus(majoitusId);
                if(majoitus != null && onkoVoimassaOleviaVarauksia(majoitus))
                {
                    majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Red;
                    majoituskohteeniInfoLbl.Text = "Majoituskohteen poistaminen epäonnistui. Majoituksella on aktiivisia varauksia.";
                    return;
                }
                

                bool succesful = MajoitusController.poistaKohde(majoitusId);
                if (succesful)
                {
                    majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Green;
                    majoituskohteeniInfoLbl.Text = "Majoituskohde poistettu järjestelmästä.";
                    for (int i = 0; i < ominaisuusList.Items.Count; i++)
                        ominaisuusList.SetItemChecked(i, false);
                    muokkaaMPk.Text = "";
                    muokkaaMHinta.Text = "";
                    muokkaaMHuoneet.Text = "";
                    muokkaaMVp.Text = "";
                    muokkaaMPa.Text = "";
                    muokkaaMRv.Text = "";
                    muokkaaMLt.Text = "";
                    BindMajoituskohteeni();
                }
                else
                {
                    majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Red;
                    majoituskohteeniInfoLbl.Text = "Majoituskohteen poistaminen epäonnistui.";
                }
            }

        }
        private void tyhjennaBtn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ominaisuusList.Items.Count; i++)
                ominaisuusList.SetItemChecked(i, false);
            muokkaaMPk.Text = "";
            muokkaaMHinta.Text = "";
            muokkaaMHuoneet.Text = "";
            muokkaaMVp.Text = "";
            muokkaaMPa.Text = "";
            muokkaaMRv.Text = "";
            muokkaaMLt.Text = "";
        }

        private void lisaaMBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string paikkakunta = muokkaaMPk.Text;
                int hinta = int.Parse(muokkaaMHinta.Text);
                int huoneet = int.Parse(muokkaaMHuoneet.Text);
                int vuodepaikat = int.Parse(muokkaaMVp.Text);
                int pintaAla = int.Parse(muokkaaMPa.Text);
                int rakennusvuosi = int.Parse(muokkaaMRv.Text);
                string lisatiedot = muokkaaMLt.Text;
               

                bool succesful = MajoitusController.lisaaMajoitus
                    (paikkakunta, hinta, pintaAla, huoneet, vuodepaikat, rakennusvuosi, lisatiedot, nykyinenKayttaja.getId());
                if (succesful)
                {
                    majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Green;
                    majoituskohteeniInfoLbl.Text = "Kohteen lisäys onnistui.";
                    for (int i = 0; i < ominaisuusList.Items.Count; i++)
                        ominaisuusList.SetItemChecked(i, false);
                    muokkaaMPk.Text = "";
                    muokkaaMHinta.Text = "";
                    muokkaaMHuoneet.Text = "";
                    muokkaaMVp.Text = "";
                    muokkaaMPa.Text = "";
                    muokkaaMRv.Text = "";
                    muokkaaMLt.Text = "";
                    BindMajoituskohteeni();
                }
                else
                {
                    majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Red;
                    majoituskohteeniInfoLbl.Text = "Kohteen lisäys epäonnistui.";
                }
            }
            catch (Exception ex)
            {
                majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Red;
                majoituskohteeniInfoLbl.Text = "Tarkista syöttämäsi tiedot.";
            }
        }

        private bool onkoVoimassaOleviaVarauksia(Majoitus majoitus)
        {
            foreach(Varaus v in majoitus.getVaraukset())
            {
                if (v.varausLoppuuDateTime() >= DateTime.Now)
                    return true;
            }
            return false;
        }

        private void majoituskohteeniDataGW_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGW = (DataGridView)sender;
            if(dataGW.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = dataGW.SelectedRows[0];
                int majoitusId = (int)selectedRow.Cells[0].Value;
                string paikkakunta = selectedRow.Cells[1].Value.ToString();
                string hinta = selectedRow.Cells[2].Value.ToString();
                string huoneet = selectedRow.Cells[3].Value.ToString();
                string vuodepaikat = selectedRow.Cells[4].Value.ToString();
                string pintaAla = selectedRow.Cells[5].Value.ToString();
                string rakennusvuosi = selectedRow.Cells[6].Value.ToString();
                string lisatiedot = selectedRow.Cells[7].Value.ToString();

                lisaaOminPanel.Visible = true;
                ominInfoLbl.Text = "";
                for (int i = 0; i < ominaisuusList.Items.Count; i++)
                {
                    var item = ominaisuusList.Items[i];
                    ominaisuusList.SetItemChecked(i, false);
                }
                List<Lisaominaisuus> ominaisuudet = OminaisuusController.haeOminaisuudet(majoitusId);
                List<string> ominaisuudetString = ominaisuudet.Select(_ => _.getNimi()).ToList();
                for(int i = 0; i < ominaisuusList.Items.Count; i++)
                {
                    var item = ominaisuusList.Items[i];
                    if (ominaisuudetString.Contains(item))
                        ominaisuusList.SetItemChecked(i, true);
                }

                muokkaaMPk.Text = paikkakunta;
                muokkaaMHinta.Text = hinta;
                muokkaaMHuoneet.Text = huoneet;
                muokkaaMVp.Text = vuodepaikat;
                muokkaaMPa.Text = pintaAla;
                muokkaaMRv.Text = rakennusvuosi;
                muokkaaMLt.Text = lisatiedot;

            }
            else
            {
                lisaaOminPanel.Visible = false;
            }

            //if (selectedRows > 0)
            //    majoituskohteeniRemoveBtn.Enabled = true;
            //else
            //    majoituskohteeniRemoveBtn.Enabled = false;
        }

        private void lisaaOminBtn_Click(object sender, EventArgs e)
        {
            int majoitusId = (int)majoituskohteeniDataGW.SelectedRows[0].Cells[0].Value;
            List<string> ominaisuudet = new List<string>();
            foreach (var item in ominaisuusList.CheckedItems)
                ominaisuudet.Add(item.ToString());

            bool succesful = OminaisuusController.lisaaOminaisuudet(ominaisuudet, majoitusId);
            if (succesful)
            {
                ominInfoLbl.ForeColor = System.Drawing.Color.Green;
                ominInfoLbl.Text = "Ominaisuudet päivitetty.";
            }
            else
            {
                ominInfoLbl.ForeColor = System.Drawing.Color.Red;
                ominInfoLbl.Text = "Ominaisuuksien päivitys epäonnistui.";
            }
        }

        private void muokkaaMBtn_Click(object sender, EventArgs e)
        {
            if(majoituskohteeniDataGW.SelectedRows.Count == 1)
            {
                try
                {
                    int majoitusId = (int)majoituskohteeniDataGW.SelectedRows[0].Cells[0].Value;
                    string paikkakunta = muokkaaMPk.Text;
                    int hinta = int.Parse(muokkaaMHinta.Text);
                    int huoneet = int.Parse(muokkaaMHuoneet.Text);
                    int vuodepaikat = int.Parse(muokkaaMVp.Text);
                    int pintaAla = int.Parse(muokkaaMPa.Text);
                    int rakennusvuosi = int.Parse(muokkaaMRv.Text);
                    string lisatiedot = muokkaaMLt.Text;
                   

                    bool succesful = MajoitusController.muokkaaMajoitusta
                        (majoitusId, paikkakunta, hinta, pintaAla, huoneet, vuodepaikat, rakennusvuosi, lisatiedot);

                    if (succesful)
                    {
                        majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Green;
                        majoituskohteeniInfoLbl.Text = "Päivitys onnistui.";
                        muokkaaMPk.Text = "";
                        muokkaaMHinta.Text = "";
                        muokkaaMHuoneet.Text = "";
                        muokkaaMVp.Text = "";
                        muokkaaMPa.Text = "";
                        muokkaaMRv.Text = "";
                        muokkaaMLt.Text = "";
                        BindMajoituskohteeni();
                    }
                    else
                    {
                        majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Red;
                        majoituskohteeniInfoLbl.Text = "Päivitys epäonnistui.";
                    }
                }
                catch (Exception ex)
                {
                    majoituskohteeniInfoLbl.ForeColor = System.Drawing.Color.Red;
                    majoituskohteeniInfoLbl.Text = "Tarkista syöttämäsi tiedot.";
                }
            }

        }

        private void majoituskohteeniPanel_Enter(object sender, EventArgs e)
        {
            if (nykyinenKayttaja != null)
            {
                BindMajoituskohteeni();
                majoituskohteeniPanel.Visible = true;
                majoituskohteeniLoginLbl.Visible = false;

                muokkaaMPk.Text = "";
                muokkaaMHinta.Text = "";
                muokkaaMPa.Text = "";
                DataBindIntValues(1, 10, 1, muokkaaMHuoneet);
                DataBindIntValues(1, 10, 1, muokkaaMVp);
                muokkaaMVp.Items.Add(">10");
                muokkaaMRv.Text = "";
                muokkaaMLt.Text = "";
                ominInfoLbl.Text = "";
            }
            else
            {
                majoituskohteeniPanel.Visible = false;
                majoituskohteeniLoginLbl.Visible = true;
            }
        }
        private void BindMajoituskohteeni()
        {
            majoituskohteeniDataGW.DataSource = MajoitusController.haeKayttajanMajoitukset(nykyinenKayttaja);
        }
        #endregion

        #region VARAUKSENI
        private void varaukseniPanel_Enter(object sender, EventArgs e)
        {
            varaukseniInfoLbl.Text = "";
            if (nykyinenKayttaja != null)
            {
                BindVaraukseni(false);
                varaukseniPanel.Visible = true;
                varaukseniLoginLbl.Visible = false;

            }
            else
            {
                varaukseniPanel.Visible = false;
                varaukseniLoginLbl.Visible = true;
            }
        }

        private void varaukseniDataGW_SelectionChanged(object sender, EventArgs e)
        {
            varaukseniListBox.Items.Clear();
            if(varaukseniDataGW.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = varaukseniDataGW.SelectedRows[0];
                int majoitusId = (int)selectedRow.Cells[3].Value;
                int varausId = (int)selectedRow.Cells[0].Value;
                string varausAlkaa = selectedRow.Cells[1].Value.ToString();
                string varausLoppuu = selectedRow.Cells[2].Value.ToString();
                Majoitus majoitus = MajoitusController.haeMajoitus(majoitusId);

                string[] varausAlkaaSplit = varausAlkaa.Split(' ');
                string[] vDate = varausAlkaaSplit[0].Split('.');
                string[] vTime = varausAlkaaSplit[1].Split('.');

                if (DateTime.Now >= new DateTime(int.Parse(vDate[2]), int.Parse(vDate[1]), int.Parse(vDate[0]), int.Parse(vTime[0]), int.Parse(vTime[1]), int.Parse(vTime[2])))
                    arvioiBtn.Enabled = true;
                else
                    arvioiBtn.Enabled = false;


                if(majoitus != null)
                {
                    varaukseniListBox.Items.Add($"Varaus alkaa: {varausAlkaa}");
                    varaukseniListBox.Items.Add($"Varaus loppuu: {varausLoppuu}");
                    varaukseniListBox.Items.Add($"Majoituksen sijainti: {majoitus.getPaikkakunta()}");
                    varaukseniListBox.Items.Add($"Majoituksen hinta: {majoitus.getHinta()} €/vrk");
                    varaukseniListBox.Items.Add($"Majoituksen lisätiedot: {majoitus.getLisatiedot()}");
                }
            }
        }

        private void varaukseniRemoveVarausBtn_Click(object sender, EventArgs e)
        {
            if (varaukseniDataGW.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = varaukseniDataGW.SelectedRows[0];
                int varausId = (int)selectedRow.Cells[0].Value;

                bool succesful = VarausController.poistaVaraus(varausId);
                if (succesful)
                {
                    varaukseniInfoLbl.ForeColor = System.Drawing.Color.Green;
                    varaukseniInfoLbl.Text = "Varaus on peruutettu.";
                    varaukseniListBox.Items.Clear();
                    BindVaraukseni(false);
                }
                else
                {
                    varaukseniInfoLbl.ForeColor = System.Drawing.Color.Red;
                    varaukseniInfoLbl.Text = "Varauksen peruutus epäonnistui.";
                }
            }
        }

        private void arvioiBtn_Click(object sender, EventArgs e)
        {
            if(varaukseniDataGW.SelectedRows.Count == 1)
            {
                int? arvio = null;
                if (!string.IsNullOrWhiteSpace(arvosanaDD.Text))
                    arvio = int.Parse(arvosanaDD.Text);
                string kommentti = kommenttiTxtBox.Text.Trim();
                int kayttajaId = nykyinenKayttaja.getId();
                int majoitusId = (int)varaukseniDataGW.SelectedRows[0].Cells[3].Value;
                DateTime aika = DateTime.Now;

                bool succesful = ArvosteluController.lisaaArvostelu(arvio, kommentti, aika, kayttajaId, majoitusId);
                if (succesful)
                {
                    arvosanaDD.Text = "";
                    kommenttiTxtBox.Text = "";
                    arvioiInfoLbl.ForeColor = System.Drawing.Color.Green;
                    arvioiInfoLbl.Text = "Arvio tallennettu.";
                }
                else
                {
                    arvioiInfoLbl.ForeColor = System.Drawing.Color.Red;
                    arvioiInfoLbl.Text = "Arvion tallennus epäonnistui.";
                }
            }


        }

        private void BindVaraukseni(bool voimassaOlevat)
        {
            if (voimassaOlevat)
                varaukseniDataGW.DataSource = VarausController.haeKayttajanVoimassaOlevatVaraukset(nykyinenKayttaja);
            else
                varaukseniDataGW.DataSource = VarausController.haeKayttajanVaraukset(nykyinenKayttaja);

        }


        private void varaukseniHaeKaikkiBtn_Click(object sender, EventArgs e)
        {
            if (nykyinenKayttaja != null)
                BindVaraukseni(false);
        }

        private void varaukseniVoimassaOlevatBtn_Click(object sender, EventArgs e)
        {
            if (nykyinenKayttaja != null)
                BindVaraukseni(true);
        }

        #endregion

        #region KÄYTTÄJÄNHALLINTA
        private void userMngBtn_Click(object sender, EventArgs e)
        {
            userManagementPanel.Visible = true;
            //userMngPanel.Visible = false;
            tabControl.Visible = false;

            userMngFirstNameTxtBox.Text = nykyinenKayttaja.getEtunimi();
            userMngLastNameTxtBox.Text = nykyinenKayttaja.getSukunimi();
            userMngEmailTxtBox.Text = nykyinenKayttaja.getSahkoposti();
            userMngPhoneTxtBox.Text = nykyinenKayttaja.getPuhelinnumero();
            userMngContactInfoLbl.Text = "";
            chngPwInfoLbl.Text = "";
            currPwTxtBox.Text = "";
            newPwTxtBox.Text = "";
            newPwTxtBox2.Text = "";
        }
        private void userManagementReturnBtn_Click(object sender, EventArgs e)
        {
            userManagementPanel.Visible = false;
            //userMngPanel.Visible = true;
            tabControl.Visible = true;
        }

        private void userMngContactInformationBtn_Click(object sender, EventArgs e)
        {
            int kayttajaId = nykyinenKayttaja.getId();
            string etunimi = userMngFirstNameTxtBox.Text;
            string sukunimi = userMngLastNameTxtBox.Text;
            string puhelinnumero = userMngPhoneTxtBox.Text;
            string sahkoposti = userMngEmailTxtBox.Text;

            if(RegistrationIsValid(etunimi, sukunimi, "asd", sahkoposti))
            {
                bool succesful = KayttajaController.paivitaYhteystiedot(kayttajaId, etunimi, sukunimi, puhelinnumero, sahkoposti);
                if (succesful)
                {
                    nykyinenKayttaja = new Kayttaja(kayttajaId, etunimi, sukunimi, nykyinenKayttaja.getSalasana(), puhelinnumero, sahkoposti);
                    userMngContactInfoLbl.ForeColor = System.Drawing.Color.Green;
                    userMngContactInfoLbl.Text = "Yhteystiedot päivitetty.";
                }
                else
                {
                    userMngContactInfoLbl.ForeColor = System.Drawing.Color.Red;
                    userMngContactInfoLbl.Text = "Yhteystietojen päivitys epäonnistui.";
                }
            }
            else
            {
                userMngContactInfoLbl.ForeColor = System.Drawing.Color.Red;
                userMngContactInfoLbl.Text = "Tarkista syöttämäsi tiedot.";
            }

        }

        private void chngPwBtn_Click(object sender, EventArgs e)
        {
            string nykyinenSalasana = currPwTxtBox.Text;
            string uusiSalasana = newPwTxtBox.Text;
            string uusiSalasana2 = newPwTxtBox2.Text;

            if (string.IsNullOrWhiteSpace(uusiSalasana) || string.IsNullOrWhiteSpace(uusiSalasana2) || uusiSalasana != uusiSalasana2)
            {
                chngPwInfoLbl.ForeColor = System.Drawing.Color.Red;
                chngPwInfoLbl.Text = "Salasanan muuttaminen epäonnistui.";
                return;
            }

            string uusiSalasanaHash = KayttajaController.vaihdaSalasana(nykyinenKayttaja, nykyinenSalasana, uusiSalasana);
            if (uusiSalasanaHash != null)
            {
                nykyinenKayttaja.setSalasana(uusiSalasanaHash);
                chngPwInfoLbl.ForeColor = System.Drawing.Color.Green;
                chngPwInfoLbl.Text = "Salasana muutettu.";
            }
            else
            {
                chngPwInfoLbl.ForeColor = System.Drawing.Color.Red;
                chngPwInfoLbl.Text = "Salasanan muuttaminen epäonnistui.";
            }
        }
        #endregion

        #region MAJOITUSHAKU
        private void haeMajoitustaBtn_Click(object sender, EventArgs e)
        {

            string paikkakunta = paikkakuntaHakuDD.Text;

            int? alinHinta = null;
            if (!string.IsNullOrEmpty(minHintaHaku.Text))
                alinHinta = int.Parse(minHintaHaku.Text);

            int? ylinHinta = null;
            if (!string.IsNullOrEmpty(maxHintaHaku.Text))
                ylinHinta = int.Parse(maxHintaHaku.Text);

            string huoneet = huoneetHaku.Text;
            string vuodepaikat = vuodepaikatHaku.Text;
            string rakennusvuosi = rakennettuHaku.Text;

            haeMajoitustaDataGW.DataSource = MajoitusController.haeMajoitusta(paikkakunta, alinHinta, ylinHinta, huoneet, vuodepaikat, rakennusvuosi);
        }

        private void haeMajoitustaDataGW_SelectionChanged(object sender, EventArgs e)
        {
            if(haeMajoitustaDataGW.SelectedRows.Count == 1)
            {
                haeMajoitusInfoBox.Text = "";
                DataGridViewRow selectedRow = haeMajoitustaDataGW.SelectedRows[0];
                int majoitusId = (int)selectedRow.Cells[0].Value;
                string paikkakunta = selectedRow.Cells[1].Value.ToString();
                string hinta = selectedRow.Cells[2].Value.ToString();
                string huoneet = selectedRow.Cells[3].Value.ToString();
                string vuodepaikat = selectedRow.Cells[4].Value.ToString();
                string pintaAla = selectedRow.Cells[5].Value.ToString();
                string rakennusvuosi = selectedRow.Cells[6].Value.ToString();
                string lisatiedot = selectedRow.Cells[7].Value.ToString();
                if (nykyinenKayttaja != null)
                {
                    List<Varaus> varaukset = VarausController.haeVoimassaOlevatVaraukset(majoitusId);
                    BindVaraukset(varaukset);
                    varausAlkaaDTP.MinDate = DateTime.Now;
                }
                ArvosteluViewModels arvostelut = ArvosteluController.haeArvostelut(majoitusId);
                string arviot = "";
                foreach (ArvosteluViewModel model in arvostelut.getArvostelut())
                    arviot += $"{model.Aika} {model.Etunimi} {model.Sukunimi}\nArvosana: {model.Arvio}\nKommentti: {model.Kommentti}";
                haeMajoitusInfoBox.Text = $"Paikkakunta: {paikkakunta}\nHinta: {hinta} €/vrk\nPinta-ala: {pintaAla} m^2\nHuoneet: {huoneet}\nVuodepaikat: {vuodepaikat}\nRakennusvuosi: {rakennusvuosi}\n\n{lisatiedot}\n\n{arviot}";
            }

           
        }

        private void majoitusHakuListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            haeMajoitusInfoBox.Text = "";
            ListBox listBox = (ListBox)sender;
            Majoitus majoitus = (Majoitus)listBox.SelectedItem;

            if (nykyinenKayttaja != null)
            {
              //  BindVaraukset(majoitus);
                varausAlkaaDTP.MinDate = DateTime.Now;
            }

            haeMajoitusInfoBox.Text = $"Paikkakunta: {majoitus.getPaikkakunta()}\nHinta: {majoitus.getHinta()} €/vrk\nPinta-ala: {majoitus.getPintaAla()} m^2\nHuoneet: {majoitus.getHuoneet()}\nVuodepaikat: {majoitus.getVuodepaikat()}\nRakennusvuosi: {majoitus.getRakennusvuosi()}\n\n{majoitus.getLisatiedot()}";
        }

        private void BindVaraukset(List<Varaus> varaukset)
        {
            voimassaOlevatVarauksetListBox.Items.Clear();
            foreach (Varaus v in varaukset.OrderBy(_ => _.varausAlkaaDateTime()))
                voimassaOlevatVarauksetListBox.Items.Add($"Alkaa: {v.getAlkuAika()} | Loppuu: {v.getLoppuAika()}");

        }



        private void MajoitusPage_Enter(object sender, EventArgs e)
        {
            teeVarausPanel.Visible = (nykyinenKayttaja != null);
            paikkakuntaHakuDD.Text = "";
            minHintaHaku.Text = "";
            maxHintaHaku.Text = "";
            huoneetHaku.Text = "";
            vuodepaikatHaku.Text = "";
            rakennettuHaku.Text = "";
            haeMajoitustaDataGW.DataSource = null;
            haeMajoitusInfoBox.Text = "";
            voimassaOlevatVarauksetListBox.Items.Clear();
            varausAlkaaDTP.Value = DateTime.Now;
            varausLoppuuDTP.Value = DateTime.Now.AddDays(1);
            varaaMajoitusInfo.Text = "";

            DataBindIntValues(0, 2000, 50, minHintaHaku);
            DataBindIntValues(0, 2000, 50, maxHintaHaku);
            DataBindIntValues(1, 10, 1, huoneetHaku);
            DataBindIntValues(1, 10, 1, vuodepaikatHaku);
            vuodepaikatHaku.Items.Add(">10");
            DataBindStringValues(1950, 2020, 10, rakennettuHaku);

        }

        private void DataBindIntValues(int min, int max, int step, ComboBox control)
        {
            control.Items.Clear();
            control.Items.Add("");
            for (int value = min; value <= max; value += step)
                control.Items.Add(value);
        }
        private void DataBindStringValues(int min, int max, int step, ComboBox control)
        {
            control.Items.Clear();
            control.Items.Add("");
            for (var value = min; value <= max; value += step)
                control.Items.Add($"{value} - {value + step}");
        }
        private void submitVarausBtn_Click(object sender, EventArgs e)
        {
            varaaMajoitusInfo.Text = "";
            if (haeMajoitustaDataGW.SelectedRows.Count == 1)
            {
                haeMajoitusInfoBox.Text = "";
                DataGridViewRow selectedRow = haeMajoitustaDataGW.SelectedRows[0];
                int majoitusId = (int)selectedRow.Cells[0].Value;
              

                List<Varaus> varaukset = VarausController.haeVoimassaOlevatVaraukset(majoitusId);
                bool valid = EiLeikkaa(varaukset);
                if (!valid)
                {
                    varaaMajoitusInfo.ForeColor = System.Drawing.Color.Red;
                    varaaMajoitusInfo.Text = "Varaus ei ole mahdollinen.";
                    return;
                }

                bool succesful = VarausController.varaaMajoitus(nykyinenKayttaja, majoitusId, varausAlkaaDTP.Value, varausLoppuuDTP.Value);
                if (succesful)
                {
                    varaaMajoitusInfo.ForeColor = System.Drawing.Color.Green;
                    varaaMajoitusInfo.Text = "Majoitus varattu.";
                }
                else
                {
                    varaaMajoitusInfo.ForeColor = System.Drawing.Color.Red;
                    varaaMajoitusInfo.Text = "Varaaminen epäonnistui.";
                }
            }
        }

        private bool EiLeikkaa(List<Varaus> varaukset)
        {
            DateTime varausAlkaa = varausAlkaaDTP.Value.Date;
            DateTime varausLoppuu = varausLoppuuDTP.Value.Date;
            foreach (Varaus v in varaukset.OrderBy(x => x.varausAlkaaDateTime()))
            {
                if (varausAlkaa < v.varausLoppuuDateTime() && varausAlkaa >= v.varausAlkaaDateTime())
                    return false;
                if (varausLoppuu > v.varausAlkaaDateTime() && varausLoppuu <= v.varausLoppuuDateTime())
                    return false;
            }
            return true;
        }

        private void varausAlkaaDTP_ValueChanged(object sender, EventArgs e)
        {
            varausLoppuuDTP.MinDate = varausAlkaaDTP.Value.AddDays(1);
        }


        #endregion

        #region LOGIN_PANEL
        private void logInBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(userNameTxtBox.Text) && !string.IsNullOrWhiteSpace(passwordTxtBox.Text))
            {
                string username = userNameTxtBox.Text;
                string password = passwordTxtBox.Text;

                Kayttaja kayttaja = KayttajaController.handleLogIn(username, password);
                if (kayttaja != null)
                {
                    nykyinenKayttaja = kayttaja;

                    // Succesful login, change visible panels
                    userMngPanel.Visible = true;
                    tabControl.Visible = true;
                    tabControl.SelectedTab = tabControl.TabPages[0];
                    loginRegPanel.Visible = false;
                    logInPanel.Visible = false;

                    logInErrorLbl.Text = "";
                    userNameTxtBox.Text = "";
                    passwordTxtBox.Text = "";
                }
                else
                {
                    logInErrorLbl.ForeColor = System.Drawing.Color.Red;
                    logInErrorLbl.Text = "Virheellinen sähköposti ja/tai salasana.";
                }
            }
            else
            {
                logInErrorLbl.ForeColor = System.Drawing.Color.Red;
                logInErrorLbl.Text = "Virheellinen sähköposti ja/tai salasana.";
            }

        }

        private void registrationSubmitBtn_Click(object sender, EventArgs e)
        {
            string firstname = regFnTxtBox.Text;
            string lastname = regLnTxtBox.Text;
            string password = regPwTxtBox.Text;
            string email = regEmTxtBox.Text;
            string phonenumber = regPhTxtBox.Text;

            // CHECK THAT IS VALID
            if(RegistrationIsValid(firstname, lastname, password, email))
            {
                bool succesful = KayttajaController.register(firstname, lastname, password, email, phonenumber);

                //SUCCESFUL
                if (succesful)
                {
                    regFnTxtBox.Text = "";
                    regLnTxtBox.Text = "";
                    regPwTxtBox.Text = "";
                    regEmTxtBox.Text = "";
                    regPhTxtBox.Text = "";
                    regFbLbl.ForeColor = System.Drawing.Color.Green;
                    regFbLbl.Text = "Rekisteröinti onnistui.";

                }
                else
                {
                    regFbLbl.ForeColor = System.Drawing.Color.Red;
                    regFbLbl.Text = "Rekisteröinti epäonnistui.";
                }

            }
            else
            {
                regFbLbl.ForeColor = System.Drawing.Color.Red;
                regFbLbl.Text = "Tarkista syöttämäsi tiedot";
            }


        }

        private bool RegistrationIsValid(string firstname, string lastname, string password, string email)
        {
            if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
                return false;
            return true;
        }

        private void goLogInBtn_Click(object sender, EventArgs e)
        {
            logInPanel.Visible = true;
            tabControl.Visible = false;
            logInErrorLbl.Text = "";
            regFbLbl.Text = "";
        }

        private void loginPanelReturnBtn_Click(object sender, EventArgs e)
        {
            logInPanel.Visible = false;
            tabControl.Visible = true;
        }
        #endregion

      


        #region TILASTOT
        private void TilastoPage_Enter(object sender, EventArgs e)
        {
            haeYleisetTilastot();
            if(nykyinenKayttaja != null)
            {
                userStatsPanel.Visible = true;
                uMCountLbl.Visible = true;
                uVCountLbl.Visible = true;
                uACountLbl.Visible = true;
                loginStatsLbl.Text = "";
                haeKayttajanTilastot();
            }
            else
            {
                userStatsPanel.Visible = true;
                uMCountLbl.Visible = false;
                uVCountLbl.Visible = false;
                uACountLbl.Visible = false;
                loginStatsLbl.Text = "Kirjaudu sisään nähdäksesi tilastosi.";
            }

        }

        private void haeYleisetTilastot()
        {
            int mCount = MajoitusController.laskeMajoitukset();
            int vCount = VarausController.laskeVaraukset();
            int aCount = ArvosteluController.laskeArvostelut();
            int kCount = KayttajaController.laskeKayttajat();

            mCountLbl.Text = "Majoituskohteiden lukumäärä: " + (mCount != -1 ? mCount.ToString() : "0");
            vCountLbl.Text = "Varausten lukumäärä: " + (vCount != -1 ? vCount.ToString() : "0");
            aCountLbl.Text = "Arvosteluiden lukumäärä: " + (aCount != -1 ? aCount.ToString() : "0");
            kCountLbl.Text = "Käyttäjien lukumäärä: " + (kCount != -1 ? kCount.ToString() : "0");
        }

        private void haeKayttajanTilastot()
        {
            int mCount = MajoitusController.laskeKayttajanMajoitukset(nykyinenKayttaja.getId());
            int vCount = VarausController.laskeKayttajanVaraukset(nykyinenKayttaja.getId());
            int aCount = ArvosteluController.laskeKayttajanArvostelut(nykyinenKayttaja.getId());

            uMCountLbl.Text = "Majoituskohteesi: " + (mCount != -1 ? mCount.ToString() : "0");
            uVCountLbl.Text = "Tekemäsi varaukset: " + (vCount != -1 ? vCount.ToString() : "0");
            uACountLbl.Text = "Tekemäsi arvostelut: " + (aCount != -1 ? aCount.ToString() : "0");
        }
        #endregion



   


        private void logOutBtn_Click(object sender, EventArgs e)
        {
            nykyinenKayttaja = null;

            userMngPanel.Visible = false;
            teeVarausPanel.Visible = false;
            loginRegPanel.Visible = true;

            logInPanel.Visible = false;
            tabControl.Visible = true;
            tabControl.SelectedTab = tabControl.TabPages[0];

            varaukseniDataGW.DataSource = null;
            varaukseniListBox.Items.Clear();
            majoituskohteeniDataGW.DataSource = null;
            muokkaaMPk.Text = "";
            muokkaaMHinta.Text = "";
            muokkaaMHuoneet.Text = "";
            muokkaaMVp.Text = "";
            muokkaaMPa.Text = "";
            muokkaaMRv.Text = "";
            muokkaaMLt.Text = "";

            varaukseniPanel.Visible = false;
            varaukseniLoginLbl.Visible = true;

            majoituskohteeniPanel.Visible = false;
            majoituskohteeniLoginLbl.Visible = true;


        }

        private void EtusivuPage_Click(object sender, EventArgs e)
        {
            tabControl.Visible = true;
            logInPanel.Visible = false;
        }


    }
}
