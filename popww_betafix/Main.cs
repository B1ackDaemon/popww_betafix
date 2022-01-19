using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace popww_betafix
{
    public partial class Main : Form
    {
        string selectedPath = "";
        string file_ext = "";
        string original_elf_md5 = "683e201a78ada6d14315f4674f83ded7";
        string expected_elf_md5 = "58622b2b89491c6cff38320d79ae0459";
        string loaded_md5 = "";
        string patched_md5 = "";
        int boot_size;
        int boot_offset;

        public Main()
        {
            InitializeComponent();
        }

        private void btn_path_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "22 files (*.22)|*.22|iso files (*.iso)|*.iso|All files (*.*)|*.*";
            dialog.ShowDialog();

            if (dialog.FileName == "")
            {
                MessageBox.Show("No file was selected!", "Open file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dialog.FileName != "")
            {
                selectedPath = dialog.FileName;
                file_ext = Path.GetExtension(dialog.FileName);
                textBox_path.Text = Path.GetFileName(dialog.FileName);

                richTextBox_log.Text += "Selected file: " + textBox_path.Text + "\n";
            }
        }

        private void btn_patch_Click(object sender, EventArgs e)
        {
            if (selectedPath != "")
            {
                PatchFile(selectedPath, file_ext);
            }
            else if (selectedPath == "")
            {
                MessageBox.Show("No file was selected!", "Patch file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label_about_Click(object sender, EventArgs e)
        {
            About about_dialog = new About();
            about_dialog.Show();
        }

        void PatchFile(string path, string format)
        {
            richTextBox_log.Text += "Opening file: " + textBox_path.Text + "\n";
            if (format == ".22")
            {
                //open file
                BinaryReader boot_bin = new BinaryReader(File.Open(path, FileMode.Open));
                richTextBox_log.Text += format + " file loaded!" + "\n";
                boot_size = (int)boot_bin.BaseStream.Length;

                //checking if ELF is valid
                bool isvalid_elf = CheckBIN(boot_bin);

                if (isvalid_elf == true)
                {
                    //patching ELF
                    WriteBIN(path, 0, 0);

                    //checking patched ELF md5
                    patched_md5 = CheckMD5(path, 0, boot_size);

                    if (patched_md5 == expected_elf_md5)
                    {
                        richTextBox_log.Text += "md5 checksum matches to patched ELF file!" + "\n";
                    }
                    else if (patched_md5 != expected_elf_md5)
                    {
                        richTextBox_log.Text += "md5 checksum doesn't match to patched ELF file!" + "\n";
                    }
                }
                else if (isvalid_elf == false)
                {
                    MessageBox.Show("Can't patch, file is not valid!", "Patch file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (format == ".iso" || format == ".ISO")
            {
                //open file
                BinaryReader iso_bin = new BinaryReader(File.Open(path, FileMode.Open));
                richTextBox_log.Text += format + " file loaded!" + "\n";

                bool isvalid_iso = CheckISO(iso_bin);

                if (isvalid_iso == true)
                {
                    iso_bin.BaseStream.Position = 0x8000;
                    ulong tmp_name = 0;

                    //looking for boot.bin in ISO
                    while (tmp_name != 0x32322E3031325F53) //(SLU)S_210.22
                    {
                        tmp_name = iso_bin.ReadUInt64();
                        if (tmp_name != 0x32322E3031325F53) //(SLU)S_210.22
                        {
                            iso_bin.BaseStream.Position -= 7;
                        }
                    }

                    //storing boot.bin info offsets
                    int boot_name_offset = (int)iso_bin.BaseStream.Position - 11;
                    int boot_size_offset = boot_name_offset - 23;
                    int boot_lba_offset = boot_size_offset - 8;
                    string bootbin_name = "";
                    int boot_lba;
                    //int boot_offset;

                    richTextBox_log.Text += "boot.bin name offset: " + boot_name_offset + "\n";
                    richTextBox_log.Text += "boot.bin size offset: " + boot_size_offset + "\n";
                    richTextBox_log.Text += "boot.bin lba offset: " + boot_lba_offset + "\n";

                    //parsing boot.bin info
                    iso_bin.BaseStream.Position = boot_name_offset;
                    for (int i = 0; i < 11; i++)
                    {
                        bootbin_name += iso_bin.ReadChar();
                    }

                    iso_bin.BaseStream.Position = boot_size_offset;
                    boot_size = iso_bin.ReadInt32();
                    iso_bin.BaseStream.Position = boot_lba_offset;
                    boot_lba = iso_bin.ReadInt32();
                    boot_offset = boot_lba * 2048;
                    richTextBox_log.Text += "boot.bin name: " + bootbin_name + "\n";
                    richTextBox_log.Text += "boot.bin size: " + boot_size + "\n";
                    richTextBox_log.Text += "boot.bin lba: " + boot_lba + "\n";
                    richTextBox_log.Text += "boot.bin offset: " + boot_offset + "\n";

                    //reading boot.bin from ISO
                    iso_bin.BaseStream.Position = boot_offset;
                    byte[] tmp_byte = iso_bin.ReadBytes(boot_size);
                    MemoryStream tmp_mem = new MemoryStream();
                    tmp_mem.Write(tmp_byte, 0, tmp_byte.Length);
                    tmp_mem.Position = 0;
                    BinaryReader tmp_boot = new BinaryReader(tmp_mem);

                    //checking if it's a valid ELF
                    bool isvalid_elf = CheckBIN(tmp_boot);

                    //close streams
                    iso_bin.Close();
                    tmp_boot.Close();
                    tmp_mem.Close();

                    if (isvalid_elf == true)
                    {
                        //patching ELF
                        WriteBIN(path, boot_offset, 0);

                        //checking patched ELF md5
                        patched_md5 = CheckMD5(path, boot_offset, boot_size);

                        if (patched_md5 == expected_elf_md5)
                        {
                            richTextBox_log.Text += "md5 checksum matches to patched ELF file!" + "\n";
                        }
                        else if (patched_md5 != expected_elf_md5)
                        {
                            richTextBox_log.Text += "md5 checksum doesn't match to patched ELF file!" + "\n";
                        }
                    }
                    else if (isvalid_elf == false)
                    {
                        MessageBox.Show("Can't patch, ELF is not valid!", "Patch file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (isvalid_iso == false)
                {
                    MessageBox.Show("Can't patch, ISO is not valid!", "Patch file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        bool CheckBIN(BinaryReader tmp_bin)
        {
            bool tmp_isvalid = false;
            
            //read header
            tmp_bin.BaseStream.Position = 0;
            uint tmp_header = tmp_bin.ReadUInt32();

            //read md5
            tmp_bin.BaseStream.Position = 0;
            byte[] tmp_byte = tmp_bin.ReadBytes((int)tmp_bin.BaseStream.Length);
            MemoryStream tmp_mem = new MemoryStream();
            tmp_mem.Write(tmp_byte, 0, tmp_byte.Length);
            tmp_mem.Position = 0;
            loaded_md5 = HashingCompute.GetMD5HashFromStream(tmp_mem);

            //close streams
            tmp_mem.Close();
            tmp_bin.Close();

            if (tmp_header == 0x464C457F) //header is .ELF
            {
                //MessageBox.Show("ELF!");
                richTextBox_log.Text += ".ELF" + " header detected!" + "\n";
                richTextBox_log.Text += "File md5: " + loaded_md5 + "\n";
                if (loaded_md5 == original_elf_md5)
                {
                    richTextBox_log.Text += "md5 checksum matches to original ELF file!" + "\n";
                }
                else if (loaded_md5 != original_elf_md5)
                {
                    richTextBox_log.Text += "md5 checksum doesn't match to original ELF file!" + "\n";
                }
                tmp_isvalid = true;
            }
            else if (tmp_header == 0x5053507E) //header is ~PSP
            {
                richTextBox_log.Text += "~PSP" + " header detected!" + "\n";
                MessageBox.Show("Can't patch encrypted files!", "Patch file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (tmp_header != 0x464C457F && tmp_header != 0x5053507E) //header is not .ELF or ~PSP
            {
                richTextBox_log.Text += "Unknown file header!" + "\n";
                MessageBox.Show("Unknown file header, can't patch!", "Patch file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tmp_isvalid;
        }

        bool CheckISO(BinaryReader tmp_iso)
        {
            bool tmp_isvalid = false;

            //read first ISO magic
            tmp_iso.BaseStream.Position = 0x8001;
            string tmp_magic1 = "";
            for (int i = 0; i < 5; i++)
            {
                tmp_magic1 += tmp_iso.ReadChar();
            }
            //read second ISO magic
            tmp_iso.BaseStream.Position = 0x8801;
            string tmp_magic2 = "";
            for (int i = 0; i < 5; i++)
            {
                tmp_magic2 += tmp_iso.ReadChar();
            }
            //checking magic
            if (tmp_magic1 == "CD001" || tmp_magic2 == "CD001")
            {
                richTextBox_log.Text += "ISO magic detected!" + "\n";
                tmp_isvalid = true;
            }
            else if (tmp_magic1 != "CD001" && tmp_magic2 != "CD001")
            {
                richTextBox_log.Text += "Can't find ISO magic!" + "\n";
            }

            return tmp_isvalid;
        }

        void WriteBIN(string path, int offset, int dummybytes)
        {
            uint[] storygate_fix = new uint[31];
            //copy original function code
            storygate_fix[0] = 0x8f85dd54; //54 dd 85 8f  	   lw         a1,-0x22ac(gp)
            storygate_fix[1] = 0x0085182b; //2b 18 85 00  	   sltu       v1,a0,a1
            storygate_fix[2] = 0x14600004; //04 00 60 14  	   bne        v1,zero,0x59cde4
            storygate_fix[3] = 0x00000000; //00 00 00 00        nop
            storygate_fix[4] = 0x24a3ffff; //ff ff a3 24  	   addiu      v1,a1,-0x1
            storygate_fix[5] = 0x10000002; //02 00 00 10  	   beq        zero,zero,0x59cde8
            storygate_fix[6] = 0xaf8393f0; //f0 93 83 af  	   _sw        v1,-0x6c10(gp)
            storygate_fix[7] = 0xaf8493f0; //f0 93 84 af  	   sw         a0,-0x6c10(gp)
            //load storygate value in a2 register
            storygate_fix[8] = 0x8f8693f0; //f0 93 86 8f  	   lw         a2,-0x6c10(gp)
            //write 38 value to a3
            storygate_fix[9] = 0x24070026; //26 00 07 24  	   li         a3,0x26
            //check, if storygate value is 38
            storygate_fix[10] = 0x14c70012; //12 00 c7 14  	   bne        a2,a3,0x59ce3c
            //clear a3 register value
            storygate_fix[11] = 0x24070000; //00 00 07 24  	   _li        a3,0x0
            //write 0x7cceb4 storygate address, and store it in v0
            storygate_fix[12] = 0x3c06007c; //7c 00 06 3c  	   lui        a2,0x7c
            storygate_fix[13] = 0x34e7ceb4; //b4 ce e7 34  	   ori        a3,a3,0xceb4
            storygate_fix[14] = 0x00c71021; //21 10 c7 00  	   addu       v0,a2,a3
            //write proper storygate ID - 0x3dc40033 for storygate 38 in 0x7cceb4
            storygate_fix[15] = 0x2406003d; //3d 00 06 24  	   li         a2,0x3d
            storygate_fix[16] = 0xa0460000; //00 00 46 a0  	   sb         a2,0x0(v0)
            storygate_fix[17] = 0x240600c4; //c4 00 06 24  	   li         a2,0xc4
            storygate_fix[18] = 0xa0460001; //01 00 46 a0  	   sb         a2,0x1(v0)
            storygate_fix[19] = 0x24060000; //00 00 06 24  	   li         a2,0x0
            storygate_fix[20] = 0xa0460002; //02 00 46 a0  	   sb         a2,0x2(v0)
            storygate_fix[21] = 0x24060033; //33 00 06 24  	   li         a2,0x33
            storygate_fix[22] = 0xa0460003; //03 00 46 a0  	   sb         a2,0x3(v0)
            //clear a3 register value
            storygate_fix[23] = 0x24070000; //00 00 07 24  	   li         a3,0x0
            //write 0x7ccea0 primary weapon address, and store it in v0
            storygate_fix[24] = 0x3c06007c; //7c 00 06 3c  	   lui        a2,0x7c
            storygate_fix[25] = 0x34e7cea0; //a0 ce e7 34  	   ori        a3,a3,0xcea0
            storygate_fix[26] = 0x00c71021; //21 10 c7 00  	   addu       v0,a2,a3
            //write proper primary weapon ID - 0x5 for storygate 38 in 0x7ccea0
            storygate_fix[27] = 0x24060005; //05 00 06 24  	   li         a2,0x5
            storygate_fix[28] = 0xa0460000; //00 00 46 a0  	   sb         a2,0x0(v0)
            //return opcode
            storygate_fix[29] = 0x03e00008; //08 00 e0 03  	   jr         ra
            storygate_fix[30] = 0x00000000; //00 00 00 00        nop

            //patching boot.bin
            BinaryWriter data = new BinaryWriter(File.OpenWrite(path));

            //0x586308, make some space, patching code to use another dummy function
            data.BaseStream.Position = 0x486408 + offset;
            data.Write(0x0c167370); //70 73 16 0c  	   jal        0x59cdc0
            //0x58638C, make some space, patching code to use another dummy function
            data.BaseStream.Position = 0x48648c + offset;
            data.Write(0x0c167370); //70 73 16 0c  	   jal        0x59cdc0
            //0x5863C8, make some space, patching code to use another dummy function
            data.BaseStream.Position = 0x4864c8 + offset;
            data.Write(0x0c167370); //70 73 16 0c  	   jal        0x59cdc0
            //0x586408, make some space, patching code to use another dummy function
            data.BaseStream.Position = 0x486508 + offset;
            data.Write(0x0c167370); //70 73 16 0c  	   jal        0x59cdc0

            //0x59CDC8, write storygate fix in previous freed area:
            data.BaseStream.Position = 0x49cec8 + offset;
            for (int i = 0; i < storygate_fix.Length; i++)
            {
                data.Write(storygate_fix[i]);
            }

            //0x14af80, patch original function link to access our function
            data.BaseStream.Position = 0x4b080 + offset;
            data.Write(0x08167372); //72 73 16 08  	   j          0x59cdc8
            //0x45bd28, patch original function link to access our function
            data.BaseStream.Position = 0x35be28 + offset;
            data.Write(0x0c167372); //72 73 16 0c  	   jal        0x59cdc8


            //write zeros at eboot.bin size difference
            if (dummybytes > 0)
            {
                data.BaseStream.Position = boot_size + offset;
                for (int i = 0; i < dummybytes; i++)
                {
                    byte temp_byte = 0;
                    data.Write(temp_byte);
                }
            }

            data.Close();

            MessageBox.Show("Done!", "Patch file", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        string CheckMD5(string tmp_path, int tmp_offset, int tmp_size)
        {
            BinaryReader tmp_bin = new BinaryReader(File.Open(tmp_path, FileMode.Open));

            //read md5
            tmp_bin.BaseStream.Position = tmp_offset;
            byte[] tmp_byte = tmp_bin.ReadBytes(tmp_size);
            MemoryStream tmp_mem = new MemoryStream();
            tmp_mem.Write(tmp_byte, 0, tmp_byte.Length);
            tmp_mem.Position = 0;
            string tmp_md5 = HashingCompute.GetMD5HashFromStream(tmp_mem);
            tmp_mem.Close();
            tmp_bin.Close();

            return tmp_md5;
        }

        class HashingCompute
        {
            public static String GetMD5HashFromFile(String fileName)
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            public static String GetMD5HashFromStream(MemoryStream mem)
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(mem);


                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            public static String GetMD5HashFromBinary(BinaryReader buf)
            {
                byte[] tmp = buf.ReadBytes((int)buf.BaseStream.Length);
                MemoryStream mem = new MemoryStream();
                mem.Write(tmp, 0, tmp.Length);
                mem.Position = 0;

                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(mem);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            public static String GetSHA1HashFromFile(String fileName)
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] retVal = sha1.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
