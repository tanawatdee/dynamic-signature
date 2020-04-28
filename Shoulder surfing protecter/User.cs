using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoulder_surfing_protecter
{
    class User
    {
        public const int NUMOFENCODE = 10;
        List<sPoint>[] encode = new List<sPoint>[NUMOFENCODE + 1];
        List<sPoint> vencode = new List<sPoint>();
        byte id, curEncode = 0;
        string uName, fName, sName;

        public void startSign()
        {
            vencode.Clear();
        }

        public void addSign(short pointX, short pointY)
        {
            vencode.Add(new sPoint { X = pointX, Y = pointY });
        }

        public bool newUser(byte id, string uName, string fName, string sName)
        {
            if(File.Exists(uName+".dtw")) return false;
            this.id = id;
            this.uName = uName;
            this.fName = fName;
            this.sName = sName;
            return true;
        }

        public bool open(string uName)
        {
            if(!File.Exists(uName+".dtw")) return false;
            BinaryReader br = new BinaryReader(File.Open(uName+".dtw", FileMode.Open));
            id = br.ReadByte();
            string fsName = br.ReadString();
            fName = fsName.Split(' ')[0];
            sName = fsName.Split(' ')[1];
            br.BaseStream.Position = 33;
            for(int i = 0; i < NUMOFENCODE; i++)
            {
                encode[i] = new List<sPoint>();
                encode[i].Add(new sPoint { X = br.ReadInt16(), Y = br.ReadInt16() });
                do
                {
                    encode[i].Add(new sPoint { X = br.ReadInt16(), Y = br.ReadInt16() });
                } while ((encode[i][encode[i].Count - 1].X != 0x7fff || encode[i][encode[i].Count - 1].Y != 0) && br.BaseStream.Position != br.BaseStream.Length);
                encode[i].RemoveAt(encode[i].Count - 1);
                br.BaseStream.Position -= 4;
            }
            br.Close();
            return true;
        }

        public bool save()
        {
            if(File.Exists(uName+".dtw"))File.Delete(uName+".dtw");
            BinaryWriter bw = new BinaryWriter(File.Open(uName + ".dtw", FileMode.Create));
            bw.Write(id);
            bw.Write(fName + " " + sName);
            while(bw.BaseStream.Position < 33)
            {
                bw.Write('\0');
            }
            for (int i = 0; i < NUMOFENCODE; i++ )
            {
                for(int j = 0; j < encode[i].Count; j++)
                {
                    bw.Write(encode[i][j].X);
                    bw.Write(encode[i][j].Y);
                }
            }
            bw.Close();
            return true;
        }

        public bool check(int index)
        {
            if (encode[index] == null) return true;
            if (DTW.encFit(encode[index], vencode) != -1) return true;
            return false;
        }

        public bool create()
        {
            byte[] match = new byte[NUMOFENCODE];
            int[] err = new int[NUMOFENCODE];
            int errt, errminmax, min, max, imin = 0, imax = NUMOFENCODE -1, minmax;
            err[0] = 0;
            for (int i = 1; i < NUMOFENCODE; i++ )
            {
                err[i] = 0;
                for(int j = 0; j < i; j++)
                {
                    errt = DTW.encFit(encode[j], encode[i]);
                    if (errt == -1) errt = DTW.encFit(encode[i], encode[j]);
                    if(errt == -1)
                    {
                        match[i]++;
                        match[j]++;
                    }
                    else
                    {
                        err[i] += errt;
                        err[j] += errt;
                    }
                }
            }
            minmax = 12;
            errminmax = 1000000000;
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] < minmax) minmax = match[i];
            }
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] == minmax && err[i] < errminmax)
                {
                    errminmax = err[i];
                    imin = i;
                }
            }
            minmax = 0;
            errminmax = 0;
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] > minmax) minmax = match[i];
            }
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] == minmax && err[i] > errminmax)
                {
                    errminmax = err[i];
                    imax = i;
                }
            }
            List<sPoint> tencode;
            tencode = encode[0];
            encode[0] = encode[imin];
            encode[imin] = tencode;
            tencode = encode[NUMOFENCODE - 1];//for create only update change to 10
            encode[NUMOFENCODE - 1] = encode[imax == 0 ? imin : imax];//for create only update change to 10
            encode[imax == 0 ? imin : imax] = tencode;
            return true;
        }

        public bool update()
        {
            encode[10] = vencode;
            byte[] match = new byte[NUMOFENCODE + 1];
            int[] err = new int[NUMOFENCODE + 1];
            int errt, errminmax, min, max, imin = 0, imax = NUMOFENCODE, minmax;
            err[0] = 0;
            for (int i = 1; i < NUMOFENCODE + 1; i++)
            {
                err[i] = 0;
                for (int j = 0; j < i; j++)
                {
                    errt = DTW.encFit(encode[j], encode[i]);
                    if (errt == -1) errt = DTW.encFit(encode[i], encode[j]);
                    if (errt == -1)
                    {
                        match[i]++;
                        match[j]++;
                    }
                    else
                    {
                        err[i] += errt;
                        err[j] += errt;
                    }
                }
            }
            minmax = 12;
            errminmax = 1000000000;
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] < minmax) minmax = match[i];
            }
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] == minmax && err[i] < errminmax)
                {
                    errminmax = err[i];
                    imin = i;
                }
            }
            minmax = 0;
            errminmax = 0;
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] > minmax) minmax = match[i];
            }
            for (int i = 0; i < err.Length; i++)
            {
                if (match[i] == minmax && err[i] > errminmax)
                {
                    errminmax = err[i];
                    imax = i;
                }
            }
            List<sPoint> tencode;
            tencode = encode[0];
            encode[0] = encode[imin];
            encode[imin] = tencode;
            tencode = encode[NUMOFENCODE];//for create only update change to 10
            encode[NUMOFENCODE] = encode[imax == 0 ? imin : imax];//for create only update change to 10
            encode[imax == 0 ? imin : imax] = tencode;
            return true;
        }

        public bool addEncode()
        {
            if (curEncode >= encode.Length - 1) return false;
            encode[curEncode] = new List<sPoint>(vencode);
            curEncode++;
            return true;
        }

        public bool delEncode()
        {
            if (curEncode == 0) return false;
            encode[curEncode - 1] = null;
            curEncode--;
            return true;
        }
        
        public void zNorm()
        {
            int iprev = 0;
            for (int i = 1; i < vencode.Count; i++)
            {
                if (vencode[i].X == 0x7fff)
                {
                    DTW.zNorm(vencode, iprev, i);
                    iprev = i;
                }
            }
            DTW.zNorm(vencode, iprev, vencode.Count);
        }
    }
}
