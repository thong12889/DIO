#define _DLL_

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RCX1K
{
    public unsafe partial class frmMain : Form
    {
#if _DLL_
        [DllImport("kernel32")]
        private extern static IntPtr LoadLibrary(String DllName);

        [DllImport("kernel32")]
        private extern static IntPtr GetProcAddress(IntPtr hModule, String ProcName);

        [DllImport("kernel32")]
        private extern static bool FreeLibrary(IntPtr hModule);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_OBOB(byte O1, byte O2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_IBIBIBIW(byte *I1, byte *I2, byte *I3, ushort *I4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_OBOBOBOW(byte O1, byte O2, byte O3, ushort O4);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_IB(byte *I1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_OB(byte O1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_IW(ushort *I1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_OW(ushort O1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_ID(uint *I1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_OD(uint O1);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_NE();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool _B_IBIB(byte *I1, byte *I2);
#endif
        private List<GroupBox>       lgp = new List<GroupBox>();
        private List<CheckBox>       lchkTgl = new List<CheckBox>(), lchkDIOIso = new List<CheckBox>(), lchkDONPN = new List<CheckBox>(),
                                     lchkPOEO = new List<CheckBox>(), lchkPOEOA = new List<CheckBox>(), lchkPOEOM = new List<CheckBox>();
        private List<Panel>          lpn = new List<Panel>(), lpnOpt = new List<Panel>();
        private List<TextBox>        ltxtDINPN = new List<TextBox>(), ltxtDI = new List<TextBox>(), ltxtDO = new List<TextBox>(),
                                     ltxtDOR = new List<TextBox>(), ltxtDOM = new List<TextBox>(), ltxtDOF = new List<TextBox>(),
                                     ltxtPOEI = new List<TextBox>();
        private List<List<CheckBox>> lchkDINPN = new List<List<CheckBox>>(), lchkDO = new List<List<CheckBox>>(), lchkDOM = new List<List<CheckBox>>();
        private List<List<TextBox>>  ltxtDIN = new List<List<TextBox>>();
        private List<Button>         lbtnDI = new List<Button>(), lbtnDO = new List<Button>();
#if _DLL_
        private IntPtr               hMod;
        private _B_OBOB              Initial, SetPOEConfig;
        private _B_IBIBIBIW[]        GetDIOConfig = new _B_IBIBIBIW[2];
        private _B_OBOBOBOW[]        SetDIOConfig = new _B_OBOBOBOW[2];
        private _B_IB[]              GetDI = new _B_IB[2], GetDO = new _B_IB[2];
        private _B_OB[]              SetDO = new _B_OB[2];
        private _B_IW[]              GetDIO = new _B_IW[2];
        private _B_OW[]              SetDIO = new _B_OW[2];
        private _B_ID                GetWDT;
        private _B_OD                SetWDT;
        private _B_NE                CancelWDT;
        private _B_IBIB              GetPOEConfig;
        private _B_IB                GetPOE;
        private _B_OB                SetPOE;
#endif
                                     // DIOIso[Device_Index][Set_&_Get]
        private int[][]              DIOIso = new int[2][] { new int[2] { 1, 0 }, new int[2] { 1, 0 } };
                                     // DIONPN[Device_Index][Input_&_Output][Set_&_Get]
        private int[][][]            DIONPN = new int[2][][] { new int[2][] { new int[2] { 0, 0 }, new int[2] { 0, 0 } }, new int[2][] { new int[2] { 0, 0 }, new int[2] { 0, 0 } } },
                                     // DIOM[Device_Index][Nonisolated_&_Isolated][Set_&_Get]
                                     DIOM = new int[2][][] { new int[2][] { new int[2] { 0xFF00, 0 }, new int[1] { 0xFF } }, new int[2][] { new int[2] { 0xFF00, 0 }, new int[1] { 0xFF } } },
                                     // DIO[Device_Index][Nonisolated_&_Isolated][Set_&_Get / Set_Output_&_Get_Output_&_Get_Input]
                                     DIO = new int[2][][] { new int[2][] { new int[2] { 0, 0 }, new int[3] { 0, 0, 0 } }, new int[2][] { new int[2] { 0, 0 }, new int[3] { 0, 0, 0 } } };
        private int[]                POEA = new int[2] { 0, 0 }, POEM = new int[2] { 0, 0 }, POE = new int[4] { 0, 0, 0, 0 },
                                     WDTBuf = new int[5] { 0, 0, 0, 0, 0 };
        private int                  WDTTick = 0;
        private bool                 NoNoEnt, WrLeave = false;
        private DateTime             tWDT;

        public frmMain()
        {
            InitializeComponent();
            lgp.Add(gpDIO1);
            lgp.Add(gpDIO2);
            lgp.Add(gpWDT);
            //lgp.Add(gpPOE);
            lchkTgl.Add(chkDIO1Tgl);
            lchkTgl.Add(chkDIO2Tgl);
            lchkTgl.Add(chkWDTTgl);
            //lchkTgl.Add(chkPOETgl);
            lpn.Add(pnDIO1);
            lpn.Add(pnDIO2);
            lpn.Add(pnWDT);
            //lpn.Add(pnPOE);
            lpnOpt.Add(pnDIO1Opt);
            lpnOpt.Add(pnDIO2Opt);
            lpnOpt.Add(pnWDTOpt);
            //lpnOpt.Add(pnPOEOpt);
            lchkDIOIso.Add(chkDIO1Iso);
            lchkDIOIso.Add(chkDIO2Iso);
            ltxtDINPN.Add(txtDI1NPN);
            ltxtDINPN.Add(txtDI2NPN);
            lchkDINPN.Add(new List<CheckBox>());
            lchkDINPN.Add(new List<CheckBox>());
            lchkDINPN[0].Add(chkDI1NPN0);
            lchkDINPN[0].Add(chkDI1NPN1);
            lchkDINPN[0].Add(chkDI1NPN2);
            lchkDINPN[0].Add(chkDI1NPN3);
            lchkDINPN[0].Add(chkDI1NPN4);
            lchkDINPN[0].Add(chkDI1NPN5);
            lchkDINPN[0].Add(chkDI1NPN6);
            lchkDINPN[0].Add(chkDI1NPN7);
            lchkDINPN[1].Add(chkDI2NPN0);
            lchkDINPN[1].Add(chkDI2NPN1);
            lchkDINPN[1].Add(chkDI2NPN2);
            lchkDINPN[1].Add(chkDI2NPN3);
            lchkDINPN[1].Add(chkDI2NPN4);
            lchkDINPN[1].Add(chkDI2NPN5);
            lchkDINPN[1].Add(chkDI2NPN6);
            lchkDINPN[1].Add(chkDI2NPN7);
            ltxtDI.Add(txtDI1);
            ltxtDI.Add(txtDI2);
            ltxtDIN.Add(new List<TextBox>());
            ltxtDIN.Add(new List<TextBox>());
            ltxtDIN[0].Add(txtDI10);
            ltxtDIN[0].Add(txtDI11);
            ltxtDIN[0].Add(txtDI12);
            ltxtDIN[0].Add(txtDI13);
            ltxtDIN[0].Add(txtDI14);
            ltxtDIN[0].Add(txtDI15);
            ltxtDIN[0].Add(txtDI16);
            ltxtDIN[0].Add(txtDI17);
            ltxtDIN[0].Add(txtDI18);
            ltxtDIN[0].Add(txtDI19);
            ltxtDIN[0].Add(txtDI110);
            ltxtDIN[0].Add(txtDI111);
            ltxtDIN[0].Add(txtDI112);
            ltxtDIN[0].Add(txtDI113);
            ltxtDIN[0].Add(txtDI114);
            ltxtDIN[0].Add(txtDI115);
            ltxtDIN[1].Add(txtDI20);
            ltxtDIN[1].Add(txtDI21);
            ltxtDIN[1].Add(txtDI22);
            ltxtDIN[1].Add(txtDI23);
            ltxtDIN[1].Add(txtDI24);
            ltxtDIN[1].Add(txtDI25);
            ltxtDIN[1].Add(txtDI26);
            ltxtDIN[1].Add(txtDI27);
            ltxtDIN[1].Add(txtDI28);
            ltxtDIN[1].Add(txtDI29);
            ltxtDIN[1].Add(txtDI210);
            ltxtDIN[1].Add(txtDI211);
            ltxtDIN[1].Add(txtDI212);
            ltxtDIN[1].Add(txtDI213);
            ltxtDIN[1].Add(txtDI214);
            ltxtDIN[1].Add(txtDI215);
            lchkDONPN.Add(chkDO1NPN);
            lchkDONPN.Add(chkDO2NPN);
            ltxtDO.Add(txtDO1);
            ltxtDO.Add(txtDO2);
            ltxtDOR.Add(txtDO1R);
            ltxtDOR.Add(txtDO2R);
            lchkDO.Add(new List<CheckBox>());
            lchkDO.Add(new List<CheckBox>());
            lchkDO[0].Add(chkDO10);
            lchkDO[0].Add(chkDO11);
            lchkDO[0].Add(chkDO12);
            lchkDO[0].Add(chkDO13);
            lchkDO[0].Add(chkDO14);
            lchkDO[0].Add(chkDO15);
            lchkDO[0].Add(chkDO16);
            lchkDO[0].Add(chkDO17);
            lchkDO[0].Add(chkDO18);
            lchkDO[0].Add(chkDO19);
            lchkDO[0].Add(chkDO110);
            lchkDO[0].Add(chkDO111);
            lchkDO[0].Add(chkDO112);
            lchkDO[0].Add(chkDO113);
            lchkDO[0].Add(chkDO114);
            lchkDO[0].Add(chkDO115);
            lchkDO[1].Add(chkDO20);
            lchkDO[1].Add(chkDO21);
            lchkDO[1].Add(chkDO22);
            lchkDO[1].Add(chkDO23);
            lchkDO[1].Add(chkDO24);
            lchkDO[1].Add(chkDO25);
            lchkDO[1].Add(chkDO26);
            lchkDO[1].Add(chkDO27);
            lchkDO[1].Add(chkDO28);
            lchkDO[1].Add(chkDO29);
            lchkDO[1].Add(chkDO210);
            lchkDO[1].Add(chkDO211);
            lchkDO[1].Add(chkDO212);
            lchkDO[1].Add(chkDO213);
            lchkDO[1].Add(chkDO214);
            lchkDO[1].Add(chkDO215);
            ltxtDOM.Add(txtDO1M);
            ltxtDOM.Add(txtDO2M);
            ltxtDOF.Add(txtDO1F);
            ltxtDOF.Add(txtDO2F);
            lchkDOM.Add(new List<CheckBox>());
            lchkDOM.Add(new List<CheckBox>());
            lchkDOM[0].Add(chkDO1M0);
            lchkDOM[0].Add(chkDO1M1);
            lchkDOM[0].Add(chkDO1M2);
            lchkDOM[0].Add(chkDO1M3);
            lchkDOM[0].Add(chkDO1M4);
            lchkDOM[0].Add(chkDO1M5);
            lchkDOM[0].Add(chkDO1M6);
            lchkDOM[0].Add(chkDO1M7);
            lchkDOM[0].Add(chkDO1M8);
            lchkDOM[0].Add(chkDO1M9);
            lchkDOM[0].Add(chkDO1M10);
            lchkDOM[0].Add(chkDO1M11);
            lchkDOM[0].Add(chkDO1M12);
            lchkDOM[0].Add(chkDO1M13);
            lchkDOM[0].Add(chkDO1M14);
            lchkDOM[0].Add(chkDO1M15);
            lchkDOM[1].Add(chkDO2M0);
            lchkDOM[1].Add(chkDO2M1);
            lchkDOM[1].Add(chkDO2M2);
            lchkDOM[1].Add(chkDO2M3);
            lchkDOM[1].Add(chkDO2M4);
            lchkDOM[1].Add(chkDO2M5);
            lchkDOM[1].Add(chkDO2M6);
            lchkDOM[1].Add(chkDO2M7);
            lchkDOM[1].Add(chkDO2M8);
            lchkDOM[1].Add(chkDO2M9);
            lchkDOM[1].Add(chkDO2M10);
            lchkDOM[1].Add(chkDO2M11);
            lchkDOM[1].Add(chkDO2M12);
            lchkDOM[1].Add(chkDO2M13);
            lchkDOM[1].Add(chkDO2M14);
            lchkDOM[1].Add(chkDO2M15);
            lbtnDI.Add(btnDI1);
            lbtnDI.Add(btnDI2);
            lbtnDO.Add(btnDO1);
            lbtnDO.Add(btnDO2);
            /*
            ltxtPOEI.Add(txtPOEI0);
            ltxtPOEI.Add(txtPOEI1);
            ltxtPOEI.Add(txtPOEI2);
            ltxtPOEI.Add(txtPOEI3);
            lchkPOEO.Add(chkPOEO0);
            lchkPOEO.Add(chkPOEO1);
            lchkPOEO.Add(chkPOEO2);
            lchkPOEO.Add(chkPOEO3);
            lchkPOEOM.Add(chkPOEOM0);
            lchkPOEOM.Add(chkPOEOM1);
            lchkPOEOM.Add(chkPOEOM2);
            lchkPOEOM.Add(chkPOEOM3);
            lchkPOEOA.Add(chkPOEOA0);
            lchkPOEOA.Add(chkPOEOA1);
            lchkPOEOA.Add(chkPOEOA2);
            lchkPOEOA.Add(chkPOEOA3);
            */
        }

        private void TglChk(int idx, bool chk)
        {
            int i, j;

            lpnOpt[idx].Visible = chk;
            if (!chk)
            {
                lchkTgl[idx].Checked = false;
                lchkTgl[idx].Text = "<";
                for (i = 0, j = lgp.Count - 1; j >= 0; j--)
                {
                    lgp[j].Width = lpnOpt[j].Location.X + (chk ? lpnOpt[j].Width + lpn[j].Margin.Right + lgp[j].Padding.Right : 0);
                    if (i < lgp[j].Width) i = lgp[j].Width;
                }
                this.Width = i + 16;
            }
            lgp[idx].Enabled = chk;
        }

        private bool TryHexTxt(TextBox txt, int *val, string txtfmt)
        {
            bool[] chk = new bool[2] { false, false };
            int i;
            
            if (chk[0] = txt.Text != "")
            {
                try
                {
                    if (chk[1] = (i = Convert.ToUInt16(txt.Text, 16)) != *val) *val = i;
                }
                catch
                {
                    chk[0] = false;
                }
            }
            if (!chk[0] || chk[1]) txt.Text = (*val).ToString(txtfmt);
            return chk[1];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool[] chk = new bool[4];
#if _DLL_
            IntPtr pFunc;
#endif
            byte b1 = 0, b2 = 0, b3 = 0;
            ushort w = 0;
            uint dw = 0;
            int i, j;
#if _DLL_
            if (chk[0] = (hMod = LoadLibrary("RCX1K.dll")) != IntPtr.Zero)
            {
                if (chk[0] = (pFunc = GetProcAddress(hMod, "Initial")) != IntPtr.Zero)
                    Initial = (_B_OBOB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_OBOB));
                for (i = 0; i < 2; i++)
                {
                    j = i + 1;
                    if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "GetDIO" + j.ToString() + "Config")) != IntPtr.Zero)
                        GetDIOConfig[i] = (_B_IBIBIBIW)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_IBIBIBIW));
                    if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "SetDIO" + j.ToString() + "Config")) != IntPtr.Zero)
                        SetDIOConfig[i] = (_B_OBOBOBOW)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_OBOBOBOW));
                    if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "GetDI" + j.ToString())) != IntPtr.Zero)
                        GetDI[i] = (_B_IB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_IB));
                    if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "GetDO" + j.ToString())) != IntPtr.Zero)
                        GetDO[i] = (_B_IB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_IB));
                    if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "SetDO" + j.ToString())) != IntPtr.Zero)
                        SetDO[i] = (_B_OB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_OB));
                    if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "GetDIO" + j.ToString())) != IntPtr.Zero)
                        GetDIO[i] = (_B_IW)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_IW));
                    if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "SetDIO" + j.ToString())) != IntPtr.Zero)
                        SetDIO[i] = (_B_OW)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_OW));
                }
                if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "GetWDT")) != IntPtr.Zero)
                    GetWDT = (_B_ID)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_ID));
                if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "SetWDT")) != IntPtr.Zero)
                    SetWDT = (_B_OD)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_OD));
                if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "CancelWDT")) != IntPtr.Zero)
                    CancelWDT = (_B_NE)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_NE));
                if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "GetPOEConfig")) != IntPtr.Zero)
                    GetPOEConfig = (_B_IBIB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_IBIB));
                if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "SetPOEConfig")) != IntPtr.Zero)
                    SetPOEConfig = (_B_OBOB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_OBOB));
                if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "GetPOE")) != IntPtr.Zero)
                    GetPOE = (_B_IB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_IB));
                if (chk[0]) if (chk[0] = (pFunc = GetProcAddress(hMod, "SetPOE")) != IntPtr.Zero)
                    SetPOE = (_B_OB)Marshal.GetDelegateForFunctionPointer(pFunc, typeof(_B_OB));
            }
            if (hMod != IntPtr.Zero && !chk[0]) FreeLibrary(hMod);
            if (!chk[0]) this.Close();
            // 0: Non-Isolated; 1: Isolated. 0: NPN; 1: PNP.
            Initial(1, 0);
#endif
            for (i = 0; i < 2; i++)
            {
#if _DLL_
                if (chk[0] = GetDIOConfig[i](&b1, &b2, &b3, &w))
#else
                chk[0] = true;
#endif
                {
                    DIOIso[i][1] = DIOIso[i][0] = b1;
                    DIONPN[i][0][1] = DIONPN[i][0][0] = b2;
                    DIONPN[i][1][1] = DIONPN[i][1][0] = b3;
                    DIOM[i][0][1] = DIOM[i][0][0] = w;
                    if (DIOIso[i][1] > 0)
                    {
#if _DLL_
                        if (chk[0] = GetDI[i](&b1))
#endif
                        {
                            DIO[i][1][2] = b1;
#if _DLL_
                            if (chk[0] = GetDO[i](&b1))
#endif
                                DIO[i][1][1] = DIO[i][1][0] = b1;
                        }
                    }
#if _DLL_
                    else if (chk[0] = GetDIO[i](&w))
#else
                    else
#endif
                        DIO[i][0][1] = DIO[i][0][0] = w;
                }
                lchkTgl[i].Checked = chk[0];
            }
#if _DLL_
            if (chk[0] = GetWDT(&dw))
#else
            chk[0] = true;
#endif
                WDTBuf[4] = (int)dw;
            chkWDTTgl.Checked = chk[0];
            /*
#if _DLL_
            if (chk[0] = GetPOEConfig(&b1, &b2))
#else
            chk[0] = true;
#endif
            {
                POEA[1] = POEA[0] = b1;
                POEM[1] = POEM[0] = b2;
#if _DLL_
                if (chk[0] = GetPOE(&b1))
#else
                b1 = (byte)POE[0];
#endif
                    POE[1] = POE[0] = b1;
            }
            chkPOETgl.Checked = chk[0];
            */
            for (i = 0, j = lgp.Count - 1; j >= 0; j--)
            {
                lchkTgl[j].Text = (chk[0] = lchkTgl[j].Checked) ? ">" : "<";
                lpnOpt[j].Visible = chk[0];
                lgp[j].Width = lpnOpt[j].Location.X + (chk[0] ? lpnOpt[j].Width + lpn[j].Margin.Right + lgp[j].Padding.Right : 0);
                lgp[j].Enabled = chk[0];
                if (i < lgp[j].Width) i = lgp[j].Width;
            }
            this.Width = i + 16;
            for (i = 0; i < 2; i++)
            {
                lchkDIOIso[i].Checked = (chk[0] = DIOIso[i][0] > 0);
                ltxtDINPN[i].Text = DIONPN[i][0][0].ToString("X2");
                lchkDONPN[i].Checked = (chk[1] = DIONPN[i][1][0] > 0);
                lchkDONPN[i].Text = chk[1] ? "PNP" : "NPN";
                lchkDONPN[i].Enabled = chk[0];
                ltxtDI[i].Text = DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1].ToString(chk[0] ? "X2" : "X4");
                ltxtDO[i].Text = DIO[i][DIOIso[i][0]][0].ToString(chk[0] ? "X2" : "X4");
                ltxtDOM[i].Text = DIOM[i][DIOIso[i][0]][0].ToString(chk[0] ? "X2" : "X4");
                ltxtDOF[i].Text = (j = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
                ltxtDOR[i].Text = (j | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
                for (j = lchkDINPN[i].Count - 1; j >= 0; j--)
                {
                    lchkDINPN[i][j].Checked = (chk[1] = (DIONPN[i][0][0] >> j & 1) > 0);
                    lchkDINPN[i][j].Text = chk[1] ? "P" : "N";
                    lchkDINPN[i][j].Enabled = chk[0];
                }
                for (j = ltxtDIN[i].Count - 1; j >= 0; j--)
                {
                    if (chk[1] = j < 8 || !chk[0])
                    {
                        ltxtDIN[i][j].Text = (DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1] >> j & 1).ToString();
                        lchkDOM[i][j].Checked = (chk[2] = (DIOM[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDOM[i][j].Text = chk[2] ? "W" : "R";
                        lchkDO[i][j].Checked = (chk[3] = (DIO[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDO[i][j].Text = chk[3] ? "1" : "0";
                    }
                    else chk[2] = lchkDOM[i][j].Checked;
                    ltxtDIN[i][j].Enabled = chk[1];
                    lchkDOM[i][j].Enabled = chk[1];
                    lchkDO[i][j].Enabled = chk[1] && chk[2];
                }
            }
            /*
            txtPOEI.Text = POE[1].ToString("X1");
            txtPOEO.Text = POE[0].ToString("X1");
            txtPOEOA.Text = POEA[0].ToString("X1");
            txtPOEOM.Text = POEM[0].ToString("X1");
            txtPOEOF.Text = (POE[2] = POE[0] & ~POEA[0] & POEM[0]).ToString("X1");
            txtPOEOR.Text = (POE[3] = POE[2] | POE[1] & (POEA[0] | ~POEM[0])).ToString("X1");
            for (i = ltxtPOEI.Count - 1; i >= 0; i--)
            {
                ltxtPOEI[i].Text = (POE[1] >> i & 1) > 0 ? "1" : "0";
                lchkPOEOA[i].Checked = (chk[0] = (POEA[0] >> i & 1) > 0);
                lchkPOEOA[i].Text = chk[0] ? "A" : "M";
                lchkPOEOM[i].Checked = (chk[1] = (POEM[0] >> i & 1) > 0);
                lchkPOEOM[i].Text = chk[1] ? "W" : "R";
                lchkPOEO[i].Checked = (chk[2] = (POE[0] >> i & 1) > 0);
                lchkPOEO[i].Text = chk[2] ? "1" : "0";
                lchkPOEOM[i].Enabled = !chk[0];
                lchkPOEO[i].Enabled = !chk[0] && chk[1];
            }
            */
            btnWDTWr.Enabled = WDTBuf[4] <= 3932100 && WDTBuf[4] > 0;
            txtWDTSecs.Text = WDTBuf[4].ToString();
            txtWDTDay.Text = (WDTBuf[3] = WDTBuf[4] / 86400).ToString();
            txtWDTHr.Text = (WDTBuf[2] = WDTBuf[4] / 3600 % 24).ToString();
            txtWDTMin.Text = (WDTBuf[1] = WDTBuf[4] / 60 % 60).ToString();
            txtWDTSec.Text = (WDTBuf[0] = WDTBuf[4] % 60).ToString();
        }

        private void AllKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void btnDI_Click(object sender, EventArgs e)
        {
            bool[] chk = new bool[10] { false, false, false, false, false, false, false, false, false, false };
            byte b1 = 0, b2 = 0, b3 = 0;
            ushort w = 0;
            int i = Convert.ToInt32(((Button)sender).Tag), j;

            WrLeave = true;
            if (!(chk[0] = DIOIso[i][0] == DIOIso[i][1] && DIONPN[i][0][0] == DIONPN[i][0][1] && DIONPN[i][1][0] == DIONPN[i][1][1] && DIOM[i][0][0] == DIOM[i][0][1]))
            {
#if _DLL_
                if (chk[0] = SetDIOConfig[i]((byte)DIOIso[i][0], (byte)DIONPN[i][0][0], (byte)DIONPN[i][1][0], (ushort)DIOM[i][0][0]))
#else
                chk[0] = true;
#endif
                {
#if _DLL_
                    System.Threading.Thread.Sleep(2);
                    if (chk[0] = GetDIOConfig[i](&b1, &b2, &b3, &w))
#else
                    b1 = (byte)DIOIso[i][0];
                    b2 = (byte)DIONPN[i][0][0];
                    b3 = (byte)DIONPN[i][1][0];
                    w = (ushort)DIOM[i][0][0];
#endif
                    {
                        DIOIso[i][1] = b1;
                        DIONPN[i][0][1] = b2;
                        DIONPN[i][1][1] = b3;
                        DIOM[i][0][1] = w;
                    }
                }
                if (chk[1] = DIOIso[i][0] != DIOIso[i][1]) DIOIso[i][0] = DIOIso[i][1];
                if (chk[2] = DIONPN[i][0][0] != DIONPN[i][0][1]) DIONPN[i][0][0] = DIONPN[i][0][1];
                if (chk[3] = DIONPN[i][1][0] != DIONPN[i][1][1]) DIONPN[i][1][0] = DIONPN[i][1][1];
                if (chk[4] = DIOM[i][0][0] != DIOM[i][0][1]) DIOM[i][0][0] = DIOM[i][0][1];
            }
            chk[6] = DIOIso[i][0] > 0;
            if (chk[0])
            {
                if (chk[6])
                {
#if _DLL_
                    if (chk[0] = GetDI[i](&b1))
#endif
                        if (chk[5] = DIO[i][1][2] != b1) DIO[i][1][2] = b1;
                }
                else
                {
#if _DLL_
                    if (chk[0] = GetDIO[i](&w))
#else
                    w = (ushort)DIO[i][0][0];
#endif
                        if (chk[5] = DIO[i][0][1] != w) DIO[i][0][1] = w;
                }
            }
            if (chk[1])
                lchkDIOIso[i].Checked = chk[6];
            if (chk[2])
                ltxtDINPN[i].Text = DIONPN[i][0][0].ToString("X2");
            if (chk[3])
            {
                lchkDONPN[i].Checked = (chk[7] = DIONPN[i][1][0] > 0);
                lchkDONPN[i].Text = chk[7] ? "PNP" : "NPN";
            }
            lchkDONPN[i].Enabled = chk[0] && chk[6];
            if (chk[1] || chk[5])
                ltxtDI[i].Text = DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1].ToString(chk[6] ? "X2" : "X4");
            if (chk[1])
                ltxtDO[i].Text = DIO[i][DIOIso[i][0]][0].ToString(chk[6] ? "X2" : "X4");
            if (chk[1] || chk[4] && !chk[6] || chk[5])
            {
                if (chk[1] || chk[4] && !chk[6])
                    ltxtDOM[i].Text = DIOM[i][DIOIso[i][0]][0].ToString(chk[6] ? "X2" : "X4");
                ltxtDOF[i].Text = (j = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[6] ? "X2" : "X4");
                ltxtDOR[i].Text = (j | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[6] ? "X2" : "X4");
            }
            for (j = lchkDINPN[i].Count - 1; j >= 0; j--)
            {
                if (chk[2])
                {
                    lchkDINPN[i][j].Checked = (chk[7] = (DIONPN[i][0][0] >> j & 1) > 0);
                    lchkDINPN[i][j].Text = chk[7] ? "P" : "N";
                }
                lchkDINPN[i][j].Enabled = chk[0] && chk[6];
            }
            for (j = ltxtDIN[i].Count - 1; j >= 0; j--)
            {
                if (chk[7] = j < 8 || !chk[6])
                {
                    if (chk[1] || chk[5])
                        ltxtDIN[i][j].Text = (DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1] >> j & 1).ToString();
                    if (chk[1] || chk[4] && !chk[6])
                    {
                        lchkDOM[i][j].Checked = (chk[8] = (DIOM[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDOM[i][j].Text = chk[8] ? "W" : "R";
                    }
                    else chk[8] = lchkDOM[i][j].Checked;
                    if (chk[1])
                    {
                        lchkDO[i][j].Checked = (chk[9] = (DIO[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDO[i][j].Text = chk[9] ? "1" : "0";
                    }
                }
                else chk[8] = lchkDOM[i][j].Checked;
                ltxtDIN[i][j].Enabled = chk[0] && chk[7];
                lchkDOM[i][j].Enabled = chk[0] && chk[7];
                lchkDO[i][j].Enabled = chk[0] && chk[7] && chk[8];
            }
            TglChk(i, chk[0]);
            WrLeave = false;
        }
/*
        private void btnPOEI_Click(object sender, EventArgs e)
        {
            bool[] chk = new bool[6] { false, false, false, false, false, false };
            byte b1 = 0, b2 = 0;
            int i;

            WrLeave = true;
            if (!(chk[0] = POEA[0] == POEA[1] && POEM[0] == POEM[1]))
            {
#if _DLL_
                if (chk[0] = SetPOEConfig((byte)POEA[0], (byte)POEM[0]))
#else
                chk[0] = true;
#endif
                {
#if _DLL_
                    System.Threading.Thread.Sleep(10);
                    if (chk[0] = GetPOEConfig(&b1, &b2))
#else
                    b1 = (byte)POEA[0];
                    b2 = (byte)POEM[0];
#endif
                    {
                        POEA[1] = b1;
                        POEM[1] = b2;
                    }
                }
                if (chk[1] = POEA[0] != POEA[1]) POEA[0] = POEA[1];
                if (chk[2] = POEM[0] != POEM[1]) POEM[0] = POEM[1];
            }
            if (chk[0])
            {
#if _DLL_
                if (chk[0] = GetPOE(&b1))
#else
                b1 = (byte)POE[0];
#endif
                    if (chk[3] = POE[1] != b1) POE[1] = b1;
            }
            if (chk[3])
                txtPOEI.Text = POE[1].ToString("X1");
            if (chk[1])
                txtPOEOA.Text = POEA[0].ToString("X1");
            if (chk[2])
                txtPOEOM.Text = POEM[0].ToString("X1");
            if (chk[1] || chk[2])
                if (POE[2] != (i = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = i).ToString("X1");
            if (chk[1] || chk[2] || chk[3])
            {
                if (POE[3] != (i = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = i).ToString("X1");
                for (i = ltxtPOEI.Count - 1; i >= 0; i--)
                {
                    if (chk[3])
                        ltxtPOEI[i].Text = (POE[1] >> i & 1).ToString();
                    if (chk[1])
                    {
                        lchkPOEOA[i].Checked = (chk[4] = (POEA[0] >> i & 1) > 0);
                        lchkPOEOA[i].Text = chk[4] ? "A" : "M";
                    }
                    else chk[4] = lchkPOEOA[i].Checked;
                    if (chk[2])
                    {
                        lchkPOEOM[i].Checked = (chk[5] = (POEM[0] >> i & 1) > 0);
                        lchkPOEOM[i].Text = chk[5] ? "W" : "R";
                    }
                    else chk[5] = lchkPOEOM[i].Checked;
                    if (chk[1])
                        lchkPOEOM[i].Enabled = !chk[4];
                    if (chk[1] || chk[2])
                        lchkPOEO[i].Enabled = !chk[4] && chk[5];
                }
            }
            TglChk(3, chk[0]);
            WrLeave = false;
        }
*/
        private void txtWr_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift) NoNoEnt = true;
            else if ((e.KeyCode < Keys.A || e.KeyCode > Keys.F) &&
                     (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                     (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) &&
                      e.KeyCode != Keys.Back) NoNoEnt = true;
            else NoNoEnt = false;
        }

        private void txtTWr_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift) NoNoEnt = true;
            else if ((e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                     (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) &&
                      e.KeyCode != Keys.Back) NoNoEnt = true;
            else NoNoEnt = false;
        }

        private void txtWr_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = NoNoEnt;
        }

        private void txtWr_KeyUp(object sender, KeyEventArgs e)
        {
            AllKeyUp(sender, e);
            NoNoEnt = true;
        }

        private void txtDINPN_Leave(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(((TextBox)sender).Tag), j;
            bool chk;

            WrLeave = true;
            j = DIONPN[i][0][0];
            if (TryHexTxt(ltxtDINPN[i], &j, "X2"))
                for (DIONPN[i][0][0] = j, j = lchkDINPN[i].Count - 1; j >= 0; j--)
                {
                    lchkDINPN[i][j].Checked = (chk = (DIONPN[i][0][0] >> j & 1) > 0);
                    lchkDINPN[i][j].Text = chk ? "P" : "N";
                }
            WrLeave = false;
        }

        private void txtDO_Leave(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(((TextBox)sender).Tag), j;
            bool[] chk = new bool[2];

            WrLeave = true;
            chk[0] = DIOIso[i][0] > 0;
            j = DIO[i][DIOIso[i][0]][0];
            if (TryHexTxt(ltxtDO[i], &j, chk[0] ? "X2" : "X4"))
            {
                DIO[i][DIOIso[i][0]][0] = j;
                ltxtDOF[i].Text = (j = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
                ltxtDOR[i].Text = (j | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
                for (j = ltxtDIN[i].Count - 1; j >= 0; j--)
                    if (j < 8 || !chk[0])
                    {
                        lchkDO[i][j].Checked = (chk[1] = (DIO[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDO[i][j].Text = chk[1] ? "1" : "0";
                    }
            }
            WrLeave = false;
        }

        private void txtDOM_Leave(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(((TextBox)sender).Tag), j;
            bool[] chk = new bool[3];

            WrLeave = true;
            chk[0] = DIOIso[i][0] > 0;
            j = DIOM[i][DIOIso[i][0]][0];
            if (TryHexTxt(ltxtDOM[i], &j, chk[0] ? "X2" : "X4"))
            {
                DIOM[i][DIOIso[i][0]][0] = j;
                ltxtDOF[i].Text = (j = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
                ltxtDOR[i].Text = (j | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
                for (j = ltxtDIN[i].Count - 1; j >= 0; j--)
                {
                    if (chk[1] = j < 8 || !chk[0])
                    {
                        lchkDOM[i][j].Checked = (chk[2] = (DIOM[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDOM[i][j].Text = chk[2] ? "W" : "R";
                    }
                    else chk[2] = lchkDOM[i][j].Checked;
                    lchkDO[i][j].Enabled = chk[1] && chk[2];
                }
            }
            WrLeave = false;
        }
/*
        private void txtPOEO_Leave(object sender, EventArgs e)
        {
            int i;
            bool chk;

            WrLeave = true;
            i = POE[0];
            if (TryHexTxt(txtPOEO, &i, "X1"))
            {
                POE[0] = i;
                if (POE[2] != (i = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = i).ToString("X1");
                if (POE[3] != (i = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = i).ToString("X1");
                for (i = ltxtPOEI.Count - 1; i >= 0; i--)
                {
                    lchkPOEO[i].Checked = (chk = (POE[0] >> i & 1) > 0);
                    lchkPOEO[i].Text = chk ? "1" : "0";
                }
            }
            WrLeave = false;
        }

        private void txtPOEOA_Leave(object sender, EventArgs e)
        {
            int i;
            bool chk;

            WrLeave = true;
            i = POEA[0];
            if (TryHexTxt(txtPOEOA, &i, "X1"))
            {
                POEA[0] = i;
                if (POE[2] != (i = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = i).ToString("X1");
                if (POE[3] != (i = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = i).ToString("X1");
                for (i = ltxtPOEI.Count - 1; i >= 0; i--)
                {
                    lchkPOEOA[i].Checked = (chk = (POEA[0] >> i & 1) > 0);
                    lchkPOEOA[i].Text = chk ? "A" : "M";
                    lchkPOEO[i].Enabled = !chk && lchkPOEOM[i].Checked;
                }
            }
            WrLeave = false;
        }

        private void txtPOEOM_Leave(object sender, EventArgs e)
        {
            int i;
            bool chk;

            WrLeave = true;
            i = POEM[0];
            if (TryHexTxt(txtPOEOM, &i, "X1"))
            {
                POEM[0] = i;
                if (POE[2] != (i = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = i).ToString("X1");
                if (POE[3] != (i = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = i).ToString("X1");
                for (i = ltxtPOEI.Count - 1; i >= 0; i--)
                {
                    lchkPOEOM[i].Checked = (chk = (POEM[0] >> i & 1) > 0);
                    lchkPOEOM[i].Text = chk ? "W" : "R";
                    lchkPOEO[i].Enabled = !lchkPOEOA[i].Checked && chk;
                }
            }
            WrLeave = false;
        }
*/
        private void txtWDT_Leave(object sender, EventArgs e)
        {
            bool chk;
            TextBox txt = (TextBox)sender;
            int i = Convert.ToInt32(txt.Tag), j, k;

            if (chk = txt.Text != "")
            {
                j = i < 4 ? i < 3 ? i > 1 ? 59 : 23 : 29 : 3932100;
                k = WDTBuf[i];
                try
                {
                    if (chk = (WDTBuf[i] = Convert.ToInt32(txt.Text)) >= 0 && WDTBuf[i] < j)
                    {
                        if (i == 4)
                        {
                            txtWDTDay.Text = (WDTBuf[3] = WDTBuf[4] / 86400).ToString();
                            txtWDTHr.Text = (WDTBuf[2] = WDTBuf[4] / 3600 % 24).ToString();
                            txtWDTMin.Text = (WDTBuf[1] = WDTBuf[4] / 60 % 60).ToString();
                            txtWDTSec.Text = (WDTBuf[0] = WDTBuf[4] % 60).ToString();
                        }
                        else WDTBuf[4] = WDTBuf[3] * 86400 + WDTBuf[2] * 3600 + WDTBuf[1] * 60 + WDTBuf[0];
                    }
                    else WDTBuf[i] = k;
                }
                catch
                {
                    chk = false;
                }
                if (!chk) WDTBuf[i] = k;
            }
            if (chk)
            {
                btnWDTWr.Enabled = WDTBuf[4] <= 3932100 && WDTBuf[4] > 0;
                txtWDTSecs.Text = WDTBuf[4].ToString();
            }
            else txt.Text = WDTBuf[i].ToString();
        }

        private void btnDO_Click(object sender, EventArgs e)
        {
            bool[] chk = new bool[10] { false, false, false, false, false, false, false, false, false, false };
            byte b1 = 0, b2 = 0, b3 = 0;
            ushort w = 0;
            int i = Convert.ToInt32(((Button)sender).Tag), j;

            WrLeave = true;
            if (!(chk[0] = DIOIso[i][0] == DIOIso[i][1] && DIONPN[i][0][0] == DIONPN[i][0][1] && DIONPN[i][1][0] == DIONPN[i][1][1] && DIOM[i][0][0] == DIOM[i][0][1]))
            {
#if _DLL_
                if (chk[0] = SetDIOConfig[i]((byte)DIOIso[i][0], (byte)DIONPN[i][0][0], (byte)DIONPN[i][1][0], (ushort)DIOM[i][0][0]))
#else
                chk[0] = true;
#endif
                {
#if _DLL_
                    System.Threading.Thread.Sleep(2);
                    if (chk[0] = GetDIOConfig[i](&b1, &b2, &b3, &w))
#else
                    b1 = (byte)DIOIso[i][0];
                    b2 = (byte)DIONPN[i][0][0];
                    b3 = (byte)DIONPN[i][1][0];
                    w = (ushort)DIOM[i][0][0];
#endif
                    {
                        DIOIso[i][1] = b1;
                        DIONPN[i][0][1] = b2;
                        DIONPN[i][1][1] = b3;
                        DIOM[i][0][1] = w;
                    }
                }
                if (chk[1] = DIOIso[i][0] != DIOIso[i][1]) DIOIso[i][0] = DIOIso[i][1];
                if (chk[2] = DIONPN[i][0][0] != DIONPN[i][0][1]) DIONPN[i][0][0] = DIONPN[i][0][1];
                if (chk[3] = DIONPN[i][0][0] != DIONPN[i][0][1]) DIONPN[i][0][0] = DIONPN[i][0][1];
                if (chk[4] = DIOM[i][0][0] != DIOM[i][0][1]) DIOM[i][0][0] = DIOM[i][0][1];
            }
            chk[6] = DIOIso[i][0] > 0;
            if (chk[0] && DIOM[i][DIOIso[i][0]][0] > 0)
            {
                if (chk[6])
                {
                    if (chk[0] = DIOM[i][1][0] == 0xFF) b1 = 0;
#if _DLL_
                    else chk[0] = GetDO[i](&b1);
#endif
                    if (chk[0])
#if _DLL_
                        if (chk[0] = SetDO[i]((byte)(DIO[i][1][0] & DIOM[i][1][0] | b1 & ~DIOM[i][1][0])))
#endif
                        {
#if _DLL_
                            System.Threading.Thread.Sleep(12); // 10 (External curcuit delay) + 2
                            if (chk[0] = GetDO[i](&b1))
#endif
                                if (chk[5] = DIO[i][1][1] != b1) DIO[i][1][1] = b1;
                        }
                }
                else
                {
                    if (chk[0] = DIOM[i][0][0] == 0xFFFF) w = 0;
#if _DLL_
                    else chk[0] = GetDIO[i](&w);
#endif
                    if (chk[0])
#if _DLL_
                        if (chk[0] = SetDIO[i]((ushort)(DIO[i][0][0] & DIOM[i][0][0] | w & ~DIOM[i][0][0])))
#endif
                        {
#if _DLL_
                            System.Threading.Thread.Sleep(12); // 10 (External curcuit delay) + 2
                            if (chk[0] = GetDIO[i](&w))
#else
                            w = (ushort)DIO[i][0][0];
#endif
                                if (chk[5] = DIO[i][0][1] != w) DIO[i][0][1] = w;
                        }
                }
            }
            if (chk[1])
                lchkDIOIso[i].Checked = chk[6];
            if (chk[2])
                ltxtDINPN[i].Text = DIONPN[i][0][0].ToString("X2");
            if (chk[3])
            {
                lchkDONPN[i].Checked = (chk[7] = DIONPN[i][1][0] > 0);
                lchkDONPN[i].Text = chk[7] ? "PNP" : "NPN";
            }
            lchkDONPN[i].Enabled = chk[0] && chk[6];
            if (chk[1] || chk[5] && !chk[6])
                ltxtDI[i].Text = DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1].ToString(chk[6] ? "X2" : "X4");
            if (chk[1])
                ltxtDO[i].Text = DIO[i][DIOIso[i][0]][0].ToString(chk[6] ? "X2" : "X4");
            if (chk[1] || chk[4] && !chk[6] || chk[5])
            {
                if (chk[1] || chk[4] && !chk[6])
                    ltxtDOM[i].Text = DIOM[i][DIOIso[i][0]][0].ToString(chk[6] ? "X2" : "X4");
                ltxtDOF[i].Text = (j = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[6] ? "X2" : "X4");
                ltxtDOR[i].Text = (j | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[6] ? "X2" : "X4");
            }
            for (j = lchkDINPN[i].Count - 1; j >= 0; j--)
            {
                if (chk[2])
                {
                    lchkDINPN[i][j].Checked = (chk[7] = (DIONPN[i][0][0] >> j & 1) > 0);
                    lchkDINPN[i][j].Text = chk[7] ? "P" : "N";
                }
                lchkDINPN[i][j].Enabled = chk[0] && chk[6];
            }
            for (j = ltxtDIN[i].Count - 1; j >= 0; j--)
            {
                if (chk[7] = j < 8 || !chk[6])
                {
                    if (chk[1] || chk[5] && !chk[6])
                        ltxtDIN[i][j].Text = (DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1] >> j & 1).ToString();
                    if (chk[1] || chk[4] && !chk[6])
                    {
                        lchkDOM[i][j].Checked = (chk[8] = (DIOM[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDOM[i][j].Text = chk[8] ? "W" : "R";
                    }
                    else chk[8] = lchkDOM[i][j].Checked;
                    if (chk[1])
                    {
                        lchkDO[i][j].Checked = (chk[9] = (DIO[i][DIOIso[i][0]][0] >> j & 1) > 0);
                        lchkDO[i][j].Text = chk[9] ? "1" : "0";
                    }
                }
                else chk[8] = lchkDOM[i][j].Checked;
                ltxtDIN[i][j].Enabled = chk[0] && chk[7];
                lchkDOM[i][j].Enabled = chk[0] && chk[7];
                lchkDO[i][j].Enabled = chk[0] && chk[7] && chk[8];
            }
            TglChk(i, chk[0]);
            WrLeave = false;
        }
/*
        private void btnPOEO_Click(object sender, EventArgs e)
        {
            bool[] chk = new bool[6] { false, false, false, false, false, false };
            byte b1 = 0, b2 = 0;
            int i;

            WrLeave = true;
            if (!(chk[0] = POEA[0] == POEA[1] && POEM[0] == POEM[1]))
            {
#if _DLL_
                if (chk[0] = SetPOEConfig((byte)POEA[0], (byte)POEM[0]))
#else
                chk[0] = true;
#endif
                {
#if _DLL_
                    System.Threading.Thread.Sleep(10);
                    if (chk[0] = GetPOEConfig(&b1, &b2))
#else
                    b1 = (byte)POEA[0];
                    b2 = (byte)POEM[0];
#endif
                    {
                        POEA[1] = b1;
                        POEM[1] = b2;
                    }
                }
                if (chk[1] = POEA[0] != POEA[1]) POEA[0] = POEA[1];
                if (chk[2] = POEM[0] != POEM[1]) POEM[0] = POEM[1];
            }
            if (chk[0])
            {
                if (chk[0] = POEM[0] == 0xF) b1 = 0;
#if _DLL_
                else chk[0] = GetPOE(&b1);
#endif
                if (chk[0])
#if _DLL_
                    if (chk[0] = SetPOE((byte)(POE[0] & ~POEA[0] & POEM[0] | b1 & ~POEM[0])))
#endif
                    {
#if _DLL_
                        System.Threading.Thread.Sleep(10);
                        if (chk[0] = GetPOE(&b1))
#else
                        b1 = (byte)POE[0];
#endif
                            if (chk[3] = POE[1] != b1) POE[1] = b1;
                    }
            }
            if (chk[3])
                txtPOEI.Text = POE[1].ToString("X1");
            if (chk[1])
                txtPOEOA.Text = POEA[0].ToString("X1");
            if (chk[2])
                txtPOEOM.Text = POEM[0].ToString("X1");
            if (chk[1] || chk[2])
                if (POE[2] != (i = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = i).ToString("X1");
            if (chk[1] || chk[2] || chk[3])
            {
                if (POE[3] != (i = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = i).ToString("X1");
                for (i = ltxtPOEI.Count - 1; i >= 0; i--)
                {
                    if (chk[3])
                        ltxtPOEI[i].Text = (POE[1] >> i & 1).ToString();
                    if (chk[1])
                    {
                        lchkPOEOA[i].Checked = (chk[4] = (POEA[0] >> i & 1) > 0);
                        lchkPOEOA[i].Text = chk[4] ? "A" : "M";
                    }
                    else chk[4] = lchkPOEOA[i].Checked;
                    if (chk[2])
                    {
                        lchkPOEOM[i].Checked = (chk[5] = (POEM[0] >> i & 1) > 0);
                        lchkPOEOM[i].Text = chk[5] ? "W" : "R";
                    }
                    else chk[5] = lchkPOEOM[i].Checked;
                    if (chk[1])
                        lchkPOEOM[i].Enabled = !chk[4];
                    if (chk[1] || chk[2])
                        lchkPOEO[i].Enabled = !chk[4] && chk[5];
                }
            }
            TglChk(3, chk[0]);
            WrLeave = false;
        }
*/
        private void btnWDTWr_Click(object sender, EventArgs e)
        {
            bool chk;

            WrLeave = true;
#if _DLL_
            if (chk = SetWDT((uint)WDTBuf[4]))
#else
            chk = true;
#endif
            {
                txtWDTSecsTmr.Text = txtWDTSecs.Text;
                txtWDTDayTmr.Text = txtWDTDay.Text;
                txtWDTHrTmr.Text = txtWDTHr.Text;
                txtWDTMinTmr.Text = txtWDTMin.Text;
                txtWDTSecTmr.Text = txtWDTSec.Text;
                tWDT = DateTime.Now;
                tmr.Enabled = true;
                btnWDTWr.Enabled = false;
                btnWDTStop.Visible = true;
            }
            TglChk(2, chk);
            WrLeave = false;
        }

        private void btnWDTStop_Click(object sender, EventArgs e)
        {
            bool chk;

            WrLeave = true;
#if _DLL_
            if (chk = CancelWDT())
#else
            chk = true;
#endif
            {
                tmr.Enabled = false;
                btnWDTWr.Enabled = true;
                btnWDTStop.Visible = false;
            }
            TglChk(2, chk);
            WrLeave = false;
        }

        private void chkTgl_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '<':
                    ((CheckBox)sender).Checked = false;
                    break;
                case '>':
                    ((CheckBox)sender).Checked = true;
                    break;
            }
        }

        private void chkTgl_CheckedChange(object sender, EventArgs e)
        {
            bool chk;
            int i = Convert.ToInt32(((CheckBox)sender).Tag), j;

            if (WrLeave) return;
            lchkTgl[i].Text = (chk = lchkTgl[i].Checked) ? ">" : "<";
            lpnOpt[i].Visible = chk;
            lgp[i].Width = lpnOpt[i].Location.X + (chk ? lpnOpt[i].Width + lpn[i].Margin.Right + lgp[i].Padding.Right : 0);
            for (i = lgp[j = lgp.Count - 1].Width; j >= 0; j--)
                if (i < lgp[j].Width) i = lgp[j].Width;
            this.Width = i + 16;
        }

        private void chkWrNPNN_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'N':
                    ((CheckBox)sender).Checked = false;
                    break;
                case 'P':
                    ((CheckBox)sender).Checked = true;
                    break;
            }
        }

        private void chkWrN_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '0':
                    ((CheckBox)sender).Checked = false;
                    break;
                case '1':
                    ((CheckBox)sender).Checked = true;
                    break;
            }
        }

        private void chkWrMN_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'R':
                case 'r':
                    ((CheckBox)sender).Checked = false;
                    break;
                case 'W':
                case 'w':
                    ((CheckBox)sender).Checked = true;
                    break;
            }
        }

        private void chkDIOIso_CheckedChanged(object sender, EventArgs e)
        {
            bool[] chk = new bool[4];
            int i = Convert.ToInt32(((CheckBox)sender).Tag), j;

            if (WrLeave) return;
            DIOIso[i][0] = (chk[0] = lchkDIOIso[i].Checked) ? 1 : 0;
            lchkDONPN[i].Enabled = chk[0];
            ltxtDI[i].Text = DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1].ToString(chk[0] ? "X2" : "X4");
            ltxtDO[i].Text = DIO[i][DIOIso[i][0]][0].ToString(chk[0] ? "X2" : "X4");
            ltxtDOM[i].Text = DIOM[i][DIOIso[i][0]][0].ToString(chk[0] ? "X2" : "X4");
            ltxtDOF[i].Text = (j = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
            ltxtDOR[i].Text = (j | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
            for (j = lchkDINPN[i].Count - 1; j >= 0; j--)
                lchkDINPN[i][j].Enabled = chk[0];
            for (j = ltxtDIN[i].Count - 1; j >= 0; j--)
            {
                if (chk[1] = j < 8 || !chk[0])
                {
                    ltxtDIN[i][j].Text = (DIO[i][DIOIso[i][0]][DIOIso[i][0] + 1] >> j & 1).ToString();
                    lchkDOM[i][j].Checked = (chk[2] = (DIOM[i][DIOIso[i][0]][0] >> j & 1) > 0);
                    lchkDOM[i][j].Text = chk[2] ? "W" : "R";
                    lchkDO[i][j].Checked = (chk[3] = (DIO[i][DIOIso[i][0]][0] >> j & 1) > 0);
                    lchkDO[i][j].Text = chk[3] ? "1" : "0";
                }
                else chk[2] = lchkDOM[i][j].Checked;
                ltxtDIN[i][j].Enabled = chk[1];
                lchkDOM[i][j].Enabled = chk[1];
                lchkDO[i][j].Enabled = chk[1] && chk[2];
            }
        }

        private void chkDONPN_CheckedChanged(object sender, EventArgs e)
        {
            bool chk;
            int i = Convert.ToInt32(((CheckBox)sender).Tag);

            if (WrLeave) return;
            DIONPN[i][1][0] = (chk = lchkDONPN[i].Checked) ? 1 : 0;
            lchkDONPN[i].Text = chk ? "PNP" : "NPN";
        }

        private void chkDONPN_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'P':
                case 'p':
                    ((CheckBox)sender).Checked = false;
                    break;
                case 'N':
                case 'n':
                    ((CheckBox)sender).Checked = true;
                    break;
            }
        }

        private void chkDINPN_CheckedChanged(object sender, EventArgs e)
        {
            bool chk;
            string[] Str = ((CheckBox)sender).Tag.ToString().Split(',');
            int i = Convert.ToInt32(Str[0]), j = Convert.ToInt32(Str[1]), k = 1 << j;

            if (WrLeave) return;
            if (chk = lchkDINPN[i][j].Checked) DIONPN[i][0][0] |= k;
            else DIONPN[i][0][0] &= ~k;
            ltxtDINPN[i].Text = DIONPN[i][0][0].ToString("X2");
            lchkDINPN[i][j].Text = chk ? "P" : "N";
        }

        private void chkDO_CheckedChanged(object sender, EventArgs e)
        {
            bool[] chk = new bool[2];
            string[] Str = ((CheckBox)sender).Tag.ToString().Split(',');
            int i = Convert.ToInt32(Str[0]), j = Convert.ToInt32(Str[1]), k = 1 << j;

            if (WrLeave) return;
            chk[0] = DIOIso[i][0] > 0;
            if (chk[1] = lchkDO[i][j].Checked) DIO[i][DIOIso[i][0]][0] |= k;
            else DIO[i][DIOIso[i][0]][0] &= ~k;
            ltxtDO[i].Text = DIO[i][DIOIso[i][0]][0].ToString(chk[0] ? "X2" : "X4");
            ltxtDOF[i].Text = (k = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
            ltxtDOR[i].Text = (k | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
            lchkDO[i][j].Text = chk[1] ? "1" : "0";
        }

        private void chkDOM_CheckedChanged(object sender, EventArgs e)
        {
            bool[] chk = new bool[3];
            string[] Str = ((CheckBox)sender).Tag.ToString().Split(',');
            int i = Convert.ToInt32(Str[0]), j = Convert.ToInt32(Str[1]), k = 1 << j;

            if (WrLeave) return;
            chk[0] = DIOIso[i][0] > 0;
            if (chk[2] = lchkDOM[i][j].Checked) DIOM[i][DIOIso[i][0]][0] |= k;
            else DIOM[i][DIOIso[i][0]][0] &= ~k;
            ltxtDOM[i].Text = DIOM[i][DIOIso[i][0]][0].ToString(chk[0] ? "X2" : "X4");
            ltxtDOF[i].Text = (k = DIO[i][DIOIso[i][0]][0] & DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
            ltxtDOR[i].Text = (k | DIO[i][DIOIso[i][0]][1] & ~DIOM[i][DIOIso[i][0]][0]).ToString(chk[0] ? "X2" : "X4");
            if (chk[1] = j < 8 || !chk[0])
                lchkDOM[i][j].Text = chk[2] ? "W" : "R";
            lchkDO[i][j].Enabled = chk[1] && chk[2];
        }
/*
        private void chkPOEO_CheckedChanged(object sender, EventArgs e)
        {
            bool chk;
            int i = Convert.ToInt32(((CheckBox)sender).Tag), j = 1 << i;

            if (WrLeave) return;
            if (chk = lchkPOEO[i].Checked) POE[0] |= j;
            else POE[0] &= ~j;
            txtPOEO.Text = POE[0].ToString("X1");
            if (POE[2] != (j = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = j).ToString("X1");
            if (POE[3] != (j = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = j).ToString("X1");
            txtPOEOR.Text = (POE[3] = POE[2] | POE[1] & (POEA[0] | ~POEM[0])).ToString("X1");
            lchkPOEO[i].Text = chk ? "1" : "0";
        }

        private void chkPOEOA_CheckedChanged(object sender, EventArgs e)
        {
            bool chk;
            int i = Convert.ToInt32(((CheckBox)sender).Tag), j = 1 << i;

            if (WrLeave) return;
            if (chk = lchkPOEOA[i].Checked) POEA[0] |= j;
            else POEA[0] &= ~j;
            txtPOEOA.Text = POEA[0].ToString("X1");
            if (POE[2] != (j = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = j).ToString("X1");
            if (POE[3] != (j = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = j).ToString("X1");
            lchkPOEOA[i].Text = chk ? "A" : "M";
            lchkPOEOM[i].Enabled = !chk;
            lchkPOEO[i].Enabled = !chk && lchkPOEOM[i].Checked;
        }

        private void chkPOEOM_CheckedChanged(object sender, EventArgs e)
        {
            bool chk;
            int i = Convert.ToInt32(((CheckBox)sender).Tag), j = 1 << i;

            if (WrLeave) return;
            if (chk = lchkPOEOM[i].Checked) POEM[0] |= j;
            else POEM[0] &= ~j;
            txtPOEOM.Text = POEM[0].ToString("X1");
            if (POE[2] != (j = POE[0] & ~POEA[0] & POEM[0])) txtPOEOF.Text = (POE[2] = j).ToString("X1");
            if (POE[3] != (j = POE[2] | POE[1] & (POEA[0] | ~POEM[0]))) txtPOEOR.Text = (POE[3] = j).ToString("X1");
            txtPOEOR.Text = (POE[3] = POE[2] | POE[1] & (POEA[0] | ~POEM[0])).ToString("X1");
            lchkPOEOM[i].Text = chk ? "W" : "R";
            lchkPOEO[i].Enabled = !lchkPOEOA[i].Checked && chk;
        }
*/
        private void tmr_Tick(object sender, EventArgs e)
        {
            int i = Convert.ToInt32((DateTime.Now - tWDT).TotalSeconds);
            if (WDTTick != i)
            {
                WDTBuf[4]--;
                txtWDTSecsTmr.Text = (WDTBuf[4] >= 0 ? WDTBuf[4] : -WDTBuf[4]).ToString();
                txtWDTDayTmr.Text = (WDTBuf[4] >= 0 ? WDTBuf[4] / 86400 : -WDTBuf[4] / 86400).ToString();
                txtWDTHrTmr.Text = (WDTBuf[4] >= 0 ? WDTBuf[4] / 3600 % 24 : -WDTBuf[4] / 3600 % 24).ToString();
                txtWDTMinTmr.Text = (WDTBuf[4] >= 0 ? WDTBuf[4] / 60 % 60 : -WDTBuf[4] / 60 % 60).ToString();
                txtWDTSecTmr.Text = (WDTBuf[4] >= 0 ? WDTBuf[4] % 60 : -WDTBuf[4] % 60).ToString();
                WDTTick = i;
            }
        }
    }
}
