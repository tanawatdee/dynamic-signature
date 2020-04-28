using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows;

namespace Shoulder_surfing_protecter
{
    class DTW
    {
        const float Acc = 0.2F;
        const int SIZE = 10;
        public static int encFit(List<sPoint> encodeA, List<sPoint> encodeB)
        {
            if (encodeA == null || encodeB == null || encodeA.Count == 0 || encodeB.Count == 0) return -1;
            List<int> pA = new List<int>(), pB = new List<int>();
            for (int i = 0; i < encodeA.Count; i++)
                if (encodeA[i].X == 0x7fff) pA.Add(i);
            for (int i = 0; i < encodeB.Count; i++)
                if (encodeB[i].X == 0x7fff) pB.Add(i);
            if (pA.Count != pB.Count)
            {
                //MessageBox.Show("Number of parts not match. " + pA.Count.ToString() + " , " + pB.Count.ToString());
                return -1; //------------------------------------------- return
            }
            pA.Add(encodeA.Count);
            pB.Add(encodeB.Count);
            int err, errt = 0;
            short xAmax,
            xAmin,
            yAmax,
            yAmin,
            xBmax,
            xBmin,
            yBmax,
            yBmin,
            dAx, dAy, dBx, dBy;
            err = 0;
            for (int i = 1; i < pA.Count; i++)
            {
                xAmax = encodeA[pA[i - 1] + 1].X;
                xAmin = xAmax;
                yAmax = encodeA[pA[i - 1] + 1].Y;
                yAmin = yAmax;
                xBmax = encodeB[pB[i - 1] + 1].X;
                xBmin = xBmax;
                yBmax = encodeB[pB[i - 1] + 1].Y;
                yBmin = yBmax;
                dAx = 0; dAy = 0; dBx = 0; dBy = 0;
                for (int j = pA[i - 1] + 1; j < pA[i]; j++)
                {
                    if (encodeA[j].X > xAmax) xAmax = encodeA[j].X;
                    if (encodeA[j].X < xAmin) xAmin = encodeA[j].X;
                    if (encodeA[j].Y > yAmax) yAmax = encodeA[j].Y;
                    if (encodeA[j].Y < yAmin) yAmin = encodeA[j].Y;
                    dAx = (short)(xAmax - xAmin);
                    dAy = (short)(yAmax - yAmin);
                }
                for (int j = pB[i - 1] + 1; j < pB[i]; j++)
                {
                    if (encodeB[j].X > xBmax) xBmax = encodeB[j].X;
                    if (encodeB[j].X < xBmin) xBmin = encodeB[j].X;
                    if (encodeB[j].Y > yBmax) yBmax = encodeB[j].Y;
                    if (encodeB[j].Y < yBmin) yBmin = encodeB[j].Y;
                    dBx = (short)(xBmax - xBmin);
                    dBy = (short)(yBmax - yBmin);
                }
                errt = DTW.dtw(encodeA, pA[i - 1]+1, pA[i], encodeB, pB[i - 1]+1, pB[i], (int)(Acc * dAx), (int)(Acc * dAy));
                if (errt == -1)break;
                err += errt;
            }
            //MessageBox.Show(errt != -1 ? "ยืนยันตัวตนสำเร็จ : " + err.ToString() : "! ไม่ถูกต้อง : " + err.ToString());
            return errt != -1 ? err : -1;
        }
        public static int dtw(List<sPoint> coorA, int stA, int fnA, List<sPoint> coorB, int stB, int fnB, int errX, int errY)
        {
            int countA = fnA-stA, countB = fnB-stB, min, k, l, n, err, totalerr = 0;
            int[,] table = new int[2, countB];
            byte[,] path = new byte[countA, countB];
            byte tmppath;
            //X---------------------------------------------------------------------------------
            table[0,0] = Math.Abs(coorA[stA].X-coorB[stB].X);
            path[0,0] = 0;
            for(int i=1;i<countB;i++)
            {
                table[0,i] += table[0,i-1] + Math.Abs(coorA[stA].X-coorB[stB+i].X);
                path[0,i] = 1;
            }
            for (int i = 1; i < countA; i++)
            {
                table[i % 2, 0] = table[i % 2 == 1 ? 0 : 1, 0] + Math.Abs(coorA[stA + i].X-coorB[stB].X);
                path[i, 0] = 3;
                for (int j = 1; j < countB; j++)
                {
                    table[i % 2, j] = Math.Abs(coorA[stA + i].X-coorB[stB + j].X);
                    min = table[i % 2 == 1 ? 0 : 1, j - 1];
                    path[i, j] = 2;
                    if (table[i % 2 == 1 ? 0 : 1, j] < min)
                    {
                        min = table[i % 2 == 1 ? 0 : 1, j];
                        path[i, j] = 3;
                    }
                    if (table[i % 2, j - 1] < min)
                    {
                        min = table[i % 2, j - 1];
                        path[i, j] = 1;
                    }
                    if (path[i, j] == 3 && table[i % 2 == 1 ? 0 : 1, j] == table[i % 2, j - 1] && (float)i / j > (float)(countA) / (countB))
                        path[i, j] = 1;
                    table[i % 2, j] += min;
                }
            }
            k = countA - 1;
            l = countB - 1;
            err = 0;
            n = 0;
            while(path[k,l]!=0)
            {
                err += Math.Abs(coorA[stA + k].X - coorB[stB + l].X);
                n++;
                tmppath = path[k,l];
                switch(path[k,l])
                {
                    case 1: l--; break;
                    case 2: k--; l--; break;
                    case 3: k--; break;
                }
                if(tmppath==2)
                {
                    if ((float)err / n > errX)
                    {
                        return -1;
                    }
                    err =0;
                    n = 0;
                }
                if(path[k,l]==0||path[k,l]==1&&tmppath==3)
                {
                    err += Math.Abs(coorA[stA + k].X - coorB[stB + l].X);
                    n++;
                    if ((float)err / n > errX)
                    {
                        return -1;
                    }
                    err = Math.Abs(coorA[stA + k].X - coorB[stB + l].X);
                    n = 1;
                }
            }
            totalerr += table[(countA - 1) % 2, countB - 1];

            //Y---------------------------------------------------------------------------------
            table[0, 0] = Math.Abs(coorA[stA].Y - coorB[stB].Y);
            path[0, 0] = 0;
            for (int i = 1; i < countB; i++)
            {
                table[0, i] += table[0, i - 1] + Math.Abs(coorA[stA].Y - coorB[stB + i].Y);
                path[0, i] = 1;
            }
            for (int i = 1; i < countA; i++)
            {
                table[i % 2, 0] = table[i % 2 == 1 ? 0 : 1, 0] + Math.Abs(coorA[stA + i].Y - coorB[stB].Y);
                path[i, 0] = 3;
                for (int j = 1; j < countB; j++)
                {
                    table[i % 2, j] = Math.Abs(coorA[stA + i].Y - coorB[stB + j].Y);
                    min = table[i % 2 == 1 ? 0 : 1, j - 1];
                    path[i, j] = 2;
                    if (table[i % 2 == 1 ? 0 : 1, j] < min)
                    {
                        min = table[i % 2 == 1 ? 0 : 1, j];
                        path[i, j] = 3;
                    }
                    if (table[i % 2, j - 1] < min)
                    {
                        min = table[i % 2, j - 1];
                        path[i, j] = 1;
                    }
                    if (path[i, j] == 3 && table[i % 2 == 1 ? 0 : 1, j] == table[i % 2, j - 1] && (float)i / j > (float)(countA) / (countB))
                        path[i, j] = 1;
                    table[i % 2, j] += min;
                }
            }
            k = countA - 1;
            l = countB - 1;
            err = 0;
            n = 0;
            while (path[k, l] != 0)
            {
                err += Math.Abs(coorA[stA + k].Y - coorB[stB + l].Y);
                n++;
                tmppath = path[k, l];
                switch (path[k, l])
                {
                    case 1: l--; break;
                    case 2: k--; l--; break;
                    case 3: k--; break;
                }
                if (tmppath == 2)
                {
                    if ((float)err / n > errY)
                    {
                        return -1;
                    }
                    err = 0;
                    n = 0;
                }
                if (path[k, l] == 0 || path[k, l] == 1 && tmppath == 3)
                {
                    err += Math.Abs(coorA[stA + k].Y - coorB[stB + l].Y);
                    n++;
                    if ((float)err / n > errY)
                    {
                        return -1;
                    }
                    err = Math.Abs(coorA[stA + k].Y - coorB[stB + l].Y);
                    n = 1;
                }
            }
            totalerr += table[(countA - 1) % 2, countB - 1];
            /*int countA = fnA-stA, countB = fnB-stB;
            float[,] table = new float[2, countB];
            List<byte> spath = new List<byte>();
            float min, ratio = (float)(countB)/(countA);
            byte[,] path = new byte[countA, countB];
            table[0,0] = disp(coorA[stA], coorB[stB]);
            path[0,0] = 0;
            for(int i=1;i<countB;i++)
            {
                table[0,i] += table[0,i-1] + disp(coorA[stA],coorB[stB+i]);
                path[0,i] = 1;
            }
            for(int i=1;i<countA;i++)
            {
                table[i%2,0] = table[i%2==1?0:1,0] + disp(coorA[stA+i],coorB[stB]);
                path[i,0] = 3;
                for(int j=1;j<countB;j++)
                {
                    table[i%2,j] = disp(coorA[stA+i],coorB[stB+j]);
                    min = table[i%2==1?0:1,j-1];
                    path[i,j] = 2;
                    if(table[i%2==1?0:1,j]<min)
                    {
                        min = table[i%2==1?0:1,j];
                        path[i,j] = 3;
                    }
                    if(table[i%2,j-1]<min)
                    {
                        min = table[i%2,j-1];
                        path[i,j] = 1;
                    }
                    if (table[i % 2 == 1 ? 0 : 1, j] == table[i % 2, j - 1] && i / j > (countA) / (countB) && path[i, j] == 3)
                        path[i, j] = 1;
                    table[i%2,j] += min;
                }
            }
            int k = countA - 1, l = countB - 1;
            while(path[k,l] != 0)
            {
                spath.Add(path[k,l]);
                if (Math.Abs(coorA[stA + k].X - coorB[stB + l].X) > errX || Math.Abs(coorA[stA + k].Y - coorB[stB + l].Y) > errY)//------------------------------------------------------------------------------->ห่างเกิน
                {
                    //MessageBox.Show("Error : ห่างเกิน" +err + " A ="+(float)k/(countA) + " B="+ (float)l/(countB));
                    return -1;
                }
                if (l < k * ratio - Acc * countB || l > k * ratio + Acc * countB)//----------------------------------------------------ไม่อยู่ในช่วง
                {
                    //MessageBox.Show("Error : Path หลุด" + err + " A =" + (float)k / (countA) + " B=" + (float)l / (countB));
                    return -1;
                }
                switch(path[k,l])
                {
                    case 1: l--; break;
                    case 2: k--; l--; break;
                    case 3: k--; break;
                }
            }*/
            return totalerr;
        }
        public static void createTemplate(string fileName, string sfileName)
        {
            List<uint[]> pos = new List<uint[]>();
            uint[] tmppos = new uint[2];
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            List<sPoint>[] encode = new List<sPoint>[10];
            int min, imin, max, imax;
            fs.Position = 0;
            byte id = br.ReadByte();
            string name = br.ReadString();
            fs.Position = 33;
            pos.Add(new uint[] { 0, (uint)fs.Position });
            while (fs.Position < fs.Length)
            {
                if (br.ReadInt16() == 0x7fff && br.ReadInt16() == 0x0000)
                {
                    pos[pos.Count - 1][1] = (uint)fs.Position - 4;
                    pos.Add(new uint[] { (uint)fs.Position - 4, 0 });
                }
            }
            pos[pos.Count - 1][1] = (uint)fs.Position;
            if (pos.Count == 1) return;
            int[] err = new int[10];
            int errt;
            for (int i = 1; i <= 10;i++ )
            {
                if (fs != null)
                {
                    encode[i - 1] = new List<sPoint>();
                    fs.Position = pos[i][0];
                    while (fs.Position < pos[i][1])
                    {
                        encode[i - 1].Add(new sPoint { X = br.ReadInt16(), Y = br.ReadInt16() });
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = i + 1; j < 10; j++)
                {
                    errt = encFit(encode[i], encode[j]);
                    if (errt == -1)
                        errt = encFit(encode[j], encode[i]);
                    if (errt == -1)
                    {
                        MessageBox.Show("Error : Signature not match. [" + i + "," + j + "]");
                        return;
                    }
                    err[i] += errt;
                    err[j] += errt;
                }
            }
            min = err[0];
            imin = 0;
            max = err[0];
            imax = 0;
            for(int i = 1;i<err.Length;i++)
            {
                if(err[i]<min)
                {
                    min = err[i];
                    imin = i;
                }
                else if(err[i]>max)
                {
                    max = err[i];
                    imax = i;
                }
            }
            tmppos[0] = pos[1][0];
            tmppos[1] = pos[1][1];
            pos[1][0] = pos[imin + 1][0];
            pos[1][1] = pos[imin + 1][1];
            pos[imin + 1][0] = tmppos[0];
            pos[imin + 1][1] = tmppos[1];
            tmppos[0] = pos[10][0];
            tmppos[1] = pos[10][1];
            pos[10][0] = pos[imax == 0 ? imin + 1 : imax + 1][0];
            pos[10][1] = pos[imax == 0 ? imin + 1 : imax + 1][1];
            pos[imax == 0 ? imin + 1 : imax + 1][0] = tmppos[0];
            pos[imax == 0 ? imin + 1 : imax + 1][1] = tmppos[1];
            FileStream fsw = new FileStream(sfileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryWriter bw = new BinaryWriter(fsw);
            fsw.Position = 0;
            bw.Write(id);
            bw.Write(name);
            while (fsw.Position < 33) bw.Write('\0');
            for(int i=1;i<=10;i++)
            {
                fs.Position = pos[i][0];
                while (fs.Position < pos[i][1]) bw.Write(br.ReadInt16());
            }
            bw.Close();
        }
        public static void upDateTemplate(string fileName, List<sPoint> Vencode)
        {
            List<uint[]> pos = new List<uint[]>();
            uint[] tmppos = new uint[2];
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            BinaryWriter bw = new BinaryWriter(fs);
            List<sPoint>[] encode = new List<sPoint>[11];
            byte[] match = new byte[11];
            int minmax, errminmax, imin = 0, imax = 10;
            fs.Position = fs.Length;
            foreach(sPoint point in Vencode)
            {
                bw.Write(point.X);
                bw.Write(point.Y);
            }
            fs.Position = 0;
            byte id = br.ReadByte();
            string name = br.ReadString();
            fs.Position = 33;
            pos.Add(new uint[] { 0, (uint)fs.Position });
            while (fs.Position < fs.Length)
            {
                if (br.ReadInt16() == 0x7fff && br.ReadInt16() == 0x0000)
                {
                    pos[pos.Count - 1][1] = (uint)fs.Position - 4;
                    pos.Add(new uint[] { (uint)fs.Position - 4, 0 });
                }
            }
            pos[pos.Count - 1][1] = (uint)fs.Position;
            if (pos.Count == 1) return;
            int[] err = new int[11];
            int errt;
            for (int i = 1; i <= 11; i++)
            {
                if (fs != null)
                {
                    encode[i - 1] = new List<sPoint>();
                    fs.Position = pos[i][0];
                    while (fs.Position < pos[i][1])
                    {
                        encode[i - 1].Add(new sPoint { X = br.ReadInt16(), Y = br.ReadInt16() });
                    }
                }
            }
            for (int i = 0; i < 11; i++)
            {
                for (int j = i + 1; j < 11; j++)
                {
                    errt = encFit(encode[i], encode[j]);
                    if (errt == -1)
                        errt = encFit(encode[j], encode[i]);
                    if (errt == -1)
                    {
                        match[i]++;
                        match[j]++;
                        continue;
                        //MessageBox.Show("Error : Signature not match. [" + i + "," + j + "]");
                        //br.Close();
                        //bw.Close();
                        //return;
                    }
                    err[i] += errt;
                    err[j] += errt;
                }
            }
            //-----------------------------------------------------------------------
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
            //min = err[0];
            //imin = 0;
            //max = err[0];
            //imax = 0;
            //for (int i = 1; i < err.Length; i++)
            //{
            //    if (err[i] < min)
            //    {
            //        min = err[i];
            //        imin = i;
            //    }
            //    else if (err[i] > max)
            //    {
            //        max = err[i];
            //        imax = i;
            //    }
            //}
            //------------------------------------------------------------------------
            tmppos[0] = pos[1][0];
            tmppos[1] = pos[1][1];
            pos[1][0] = pos[imin + 1][0];
            pos[1][1] = pos[imin + 1][1];
            pos[imin + 1][0] = tmppos[0];
            pos[imin + 1][1] = tmppos[1];
            tmppos[0] = pos[11][0];
            tmppos[1] = pos[11][1];
            pos[11][0] = pos[imax == 0 ? imin + 1 : imax + 1][0];
            pos[11][1] = pos[imax == 0 ? imin + 1 : imax + 1][1];
            pos[imax == 0 ? imin + 1 : imax + 1][0] = tmppos[0];
            pos[imax == 0 ? imin + 1 : imax + 1][1] = tmppos[1];
            FileStream fsw = new FileStream("tmp.tmp", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            bw = new BinaryWriter(fsw);
            fsw.Position = 0;
            bw.Write(id);
            bw.Write(name);
            while (fsw.Position < 33) bw.Write('\0');
            for (int i = 1; i <= 10; i++)
            {
                fs.Position = pos[i][0];
                while (fs.Position < pos[i][1]) bw.Write(br.ReadInt16());
            }
            br.Close();
            bw.Close();
            File.Delete(fileName);
            File.Move("tmp.tmp", fileName);
        }

        public static void zNorm(List<sPoint> coorA, int stA, int fnA)
        {
            int sum = 0;
            int n = fnA - stA - 1;
            for (int j = stA + 1; j < fnA; j++)
            {
                sum += coorA[j].X;
            }
            int mean = sum / n;
            sum = 0;
            for (int j = stA + 1; j < fnA; j++)
            {
                sum += (coorA[j].X - mean) * (coorA[j].X - mean);
            }
            int sd = (int)(Math.Sqrt(sum) / n);
            for (int j = stA + 1; j < fnA; j++)
            {
                coorA[j] = new sPoint { X = (short)((float)(coorA[j].X - mean) / sd * SIZE), Y = coorA[j].Y };
            }
            sum = 0;
            for (int j = stA + 1; j < fnA; j++)
            {
                sum += coorA[j].Y;
            }
            mean = sum / n;
            sum = 0;
            for (int j = stA + 1; j < fnA; j++)
            {
                sum += (coorA[j].Y - mean) * (coorA[j].Y - mean);
            }
            sd = (int)(Math.Sqrt(sum) / n);
            for (int j = stA + 1; j < fnA; j++)
            {
                coorA[j] = new sPoint { X = coorA[j].X, Y = (short)((float)(coorA[j].Y - mean) / sd * SIZE) };
            }
        }
    }
}
