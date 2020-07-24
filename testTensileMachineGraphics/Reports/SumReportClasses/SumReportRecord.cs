using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics.Reports.SumReportClasses
{
    /// <summary>
    /// 1. u ovoj strukturi se cuva informacija o uzorku koja se upisuje u red zbirnog izvestaja
    /// i ona se cuva kada korisnik zeli da zapamti dati uzorak
    /// 2. objekat klase SumReportLastLoad predstavlja listu objekata SumReportRecord
    /// </summary>
    public class SumReportRecord
    {

        #region members

        private string _brzbIzvestaja = String.Empty;
        private string _polazniKvalitet = String.Empty;
        private string _nazivnaDebljina = String.Empty;
        private string _ispitivac = String.Empty;
        private string _brUzorka = String.Empty;
        private string _sarza = String.Empty;
        private string _rm = String.Empty;
        private string _rp02 = String.Empty;
        private string _rt05 = String.Empty;
        private string _reL = String.Empty;
        private string _reH = String.Empty;
        private string _a = String.Empty;
        private string _at = String.Empty;
        private string _n = String.Empty;
        private string _z = String.Empty;
        private string _napomena = String.Empty;

        #endregion


        #region constructors

        public SumReportRecord(string brzbIzvestaja, string polazniKvalitet, string nazivnaDebljina, string ispitivac, string brUzorka, string sarza, string rm, string rp02, string rt05, string reL, string reH, string a, string at, string n, string z, string napomena) 
        {
            try
            {
                _brzbIzvestaja = brzbIzvestaja;
                _polazniKvalitet = polazniKvalitet;
                _nazivnaDebljina = nazivnaDebljina;
                _ispitivac = ispitivac;
                _brUzorka = brUzorka;
                _sarza = sarza;
                _rm = rm;
                _rp02 = rp02;
                _rt05 = rt05;
                _reL = reL;
                _reH = reH;
                _a = a;
                _at = at;
                _n = n;
                _z = z;
                _napomena = napomena;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[SumReportRecord.cs] {public SumReportRecord(many parameters)}", System.DateTime.Now);
            }
        }

        #endregion


        #region properties

        public string BrzbIzvestaja
        {
            get { return _brzbIzvestaja; }
            set { _brzbIzvestaja = value; }
        }

        public string PolazniKvalitet
        {
            get { return _polazniKvalitet; }
            set { _polazniKvalitet = value; }
        }

        public string NazivnaDebljina
        {
            get { return _nazivnaDebljina; }
            set { _nazivnaDebljina = value; }
        }

        public string Ispitivac
        {
            get { return _ispitivac; }
            set { _ispitivac = value; }
        }

        public string BrUzorka
        {
            get { return _brUzorka; }
            set { _brUzorka = value; }
        }

        public string Sarza
        {
            get { return _sarza; }
            set { _sarza = value; }
        }

        public string Rm
        {
            get { return _rm; }
            set { _rm = value; }
        }

        public string Rp02
        {
            get { return _rp02; }
            set { _rp02 = value; }
        }

        public string Rt05
        {
            get { return _rt05; }
            set { _rt05 = value; }
        }

        public string ReL
        {
            get { return _reL; }
            set { _reL = value; }
        }

        public string ReH
        {
            get { return _reH; }
            set { _reH = value; }
        }

        public string A
        {
            get { return _a; }
            set { _a = value; }
        }

        public string At
        {
            get { return _at; }
            set { _at = value; }
        }

        public string N
        {
            get { return _n; }
            set { _n = value; }
        }

        public string Z
        {
            get { return _z; }
            set { _z = value; }
        }

        public string Napomena
        {
            get { return _napomena; }
            set { _napomena = value; }
        }

        #endregion


        #region methods

        #endregion
    }
}
