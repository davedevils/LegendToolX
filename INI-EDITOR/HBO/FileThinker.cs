using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ini_Editor
{
    class FileThinker
    {
        //GetFileRow
        public List<string> GetFileRow(string fileName)
        {
            List<string> ListRow = new List<string>();
            bool IsError = true;

            string FileWithoutini = Path.GetFileName(fileName).ToLower();

            if (FileWithoutini.Split('_').Count() > 1)
                FileWithoutini = FileWithoutini.Split('_')[1];

            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425
            binReader.Close();


            switch(FileWithoutini) 
            {
                case "kfmindex.ini": ListRow = ReadKfmIndex(fileName, VersionNumber); IsError = false; break;
                case "level.ini": ListRow = ReadLevelFile(fileName, VersionNumber); IsError = false; break;
                case "animation.ini": ListRow = ReadAnimationFile(fileName, VersionNumber); IsError = false; break;
                case "animationdepend.ini": ListRow = ReadAnimationDependFile(fileName, VersionNumber); IsError = false; break;
                case "soundid.ini": ListRow = ReadSoundIDFile(fileName, VersionNumber); IsError = false; break;
                case "gatewayarea.ini": ListRow = ReadGatewayAreaFile(fileName, VersionNumber); IsError = false; break;
                case "particleid.ini": ListRow = ReadParticleIDFile(fileName, VersionNumber); IsError = false; break;
                case "textid.ini": ListRow = ReadTextIDFile(fileName, VersionNumber); IsError = false; break;
                case "commoneffect.ini": ListRow = ReadCommonEffectFile(fileName, VersionNumber); IsError = false; break;
                case "bgm.ini": ListRow = ReadBgmFile(fileName, VersionNumber); IsError = false; break;
                case "textureid.ini": ListRow = ReadTextureIDFile(fileName, VersionNumber); IsError = false; break;
                case "shop.ini": ListRow = ReadShopFile(fileName, VersionNumber); IsError = false; break;
                case "shopmenu.ini": ListRow = ReadShopMenuFile(fileName, VersionNumber); IsError = false; break;
                case "class.ini": ListRow = ReadClassFile(fileName, VersionNumber); IsError = false; break;
                case "armortype.ini": ListRow = ReadArmorTypeFile(fileName, VersionNumber); IsError = false; break;
                case "bornarea.ini": ListRow = ReadBornAreaFile(fileName, VersionNumber); IsError = false; break;
                case "channelconfig.ini": ListRow = ReadChannelConfigFile(fileName, VersionNumber); IsError = false; break;
                case "classspellmaster.ini": ListRow = ReadClassSpellMasterFile(fileName, VersionNumber); IsError = false; break;
                case "color.ini": ListRow = ReadColorFile(fileName, VersionNumber); IsError = false; break;
                case "compound.ini": ListRow = ReadCompoundFile(fileName, VersionNumber); IsError = false; break;
                case "weapontype.ini": ListRow = ReadWeaponTypeFile(fileName, VersionNumber); IsError = false; break;
                case "cuemessage.ini": ListRow = ReadCueMessageFile(fileName, VersionNumber); IsError = false; break;
                case "effect.ini": ListRow = ReadEffectFile(fileName, VersionNumber); IsError = false; break;
                case "emotion.ini": ListRow = ReadEmotionFile(fileName, VersionNumber); IsError = false; break;
                case "envsoundfx.ini": ListRow = ReadEnvSoundFxFile(fileName, VersionNumber); IsError = false; break;
                case "eventarea.ini": ListRow = ReadEventAreaFile(fileName, VersionNumber); IsError = false; break;
                case "faceforplayer.ini": ListRow = ReadFaceForPlayerFile(fileName, VersionNumber); IsError = false; break;
                case "familyfunctioncontrol.ini": ListRow = ReadFamilyFunctionControlFile(fileName, VersionNumber); IsError = false; break;
                case "formulaparameters.ini": ListRow = ReadFormulaParametersFile(fileName, VersionNumber); IsError = false; break;
                //case "glow.ini": ListRow = ReadGlowFile(fileName, VersionNumber); IsError = false; break;
                //case "glowtype.ini": ListRow = ReadGlowTypeFile(fileName, VersionNumber); IsError = false; break;
                case "greetingdial.ini": ListRow = ReadGreetingIngDialFile(fileName, VersionNumber); IsError = false; break;
                case "hairforplayer.ini":ListRow = ReadHairForPlayerFile(fileName, VersionNumber); IsError = false; break;
                case "hbover.ini": ListRow = ReadHBOVerFile(fileName, VersionNumber); IsError = false; break;
                case "item.ini": ListRow = ReadItemFile(fileName, VersionNumber); IsError = false; break;
                case "itemaddattr.ini": ListRow = ReadItemAddAttrFile(fileName, VersionNumber); IsError = false; break;
                case "itemclasslimit.ini": ListRow = ReadItemClassLimitFile(fileName, VersionNumber); IsError = false; break;
                    
            }

            if (IsError == true)
            {
                // ERROR :'( 
                ListRow.Add( "1|0|000000|2");
                ListRow.Add("Error|Don't find the ini on list !");
            }


            return ListRow;
        }

        private List<string> ReadItemClassLimitFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|3|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    uint unknow1 = binReader.ReadUInt32();
                    string unknow2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow3 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));


                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadItemAddAttrFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|44|");

            int TotalEntry = 0;
            if (VersionNumber == "061214")
            {
                do
                {
                    uint unknow1 = binReader.ReadUInt32();
                    string unknow2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow3 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow4 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    int unknow8 = binReader.ReadInt32();
                    int unknow9 = binReader.ReadInt32();
                    int unknow10 = binReader.ReadInt32();
                    int unknow11 = binReader.ReadInt32();
                    int unknow12 = binReader.ReadInt32();
                    int unknow13 = binReader.ReadInt32();
                    int unknow14 = binReader.ReadInt32();
                    int unknow15 = binReader.ReadInt32();
                    int unknow16 = binReader.ReadInt32();
                    int unknow17 = binReader.ReadInt32();
                    int unknow18 = binReader.ReadInt32();
                    int unknow19 = binReader.ReadInt32();
                    int unknow20 = binReader.ReadInt32();
                    string unknow21 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow22 = binReader.ReadInt32();
                    int unknow23 = binReader.ReadInt32();
                    int unknow24 = binReader.ReadInt32();
                    int unknow25 = binReader.ReadInt32();
                    int unknow26 = binReader.ReadInt32();
                    int unknow27 = binReader.ReadInt32();
                    int unknow28 = binReader.ReadInt32();
                    int unknow29 = binReader.ReadInt32();
                    int unknow30 = binReader.ReadInt32();
                    int unknow31 = binReader.ReadInt32();
                    int unknow32 = binReader.ReadInt32();
                    int unknow33 = binReader.ReadInt32();
                    int unknow34 = binReader.ReadInt32();
                    int unknow35 = binReader.ReadInt32();
                    int unknow36 = binReader.ReadInt32();
                    int unknow37 = binReader.ReadInt32();
                    int unknow38 = binReader.ReadInt32();
                    int unknow39 = binReader.ReadInt32();
                    int unknow40 = binReader.ReadInt32();
                    int unknow41 = binReader.ReadInt32();
                    int unknow42 = binReader.ReadInt32();
                    int unknow43 = binReader.ReadInt32();
                    int unknow44 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknow12 + "|" + unknow13 + "|" + unknow14 + "|" + unknow15 + "|" + unknow16 + "|" + unknow17 + "|" + unknow18 + "|" + unknow19 + "|" + unknow20 + "|" + unknow21 + "|" + unknow22 + "|" + unknow23 + "|" + unknow24 + "|" + unknow25 + "|" + unknow26 + "|" + unknow27 + "|" + unknow28 + "|" + unknow29 + "|" + unknow30 + "|" + unknow31 + "|" + unknow32 + "|" + unknow33 + "|" + unknow34 + "|" + unknow35 + "|" + unknow36 + "|" + unknow37 + "|" + unknow38 + "|" + unknow39 + "|" + unknow40 + "|" + unknow41 + "|" + unknow42 + "|" + unknow43 + "|" + unknow44 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadItemFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|81|");

            int TotalEntry = 0;
            if (VersionNumber == "060609")
            {
                do
                {
                    uint unknow1 = binReader.ReadUInt32();
                    string unknow2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow3 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    int unknow8 = binReader.ReadInt32();
                    int unknow9 = binReader.ReadInt32();
                    string unknow10 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow11 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknowText = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow12 = binReader.ReadInt32();
                    int unknow13 = binReader.ReadInt32();
                    int unknow14 = binReader.ReadInt32();
                    int unknow15 = binReader.ReadInt32();
                    string unknow16 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow17 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow18 = binReader.ReadInt32();
                    int unknow19 = binReader.ReadInt32();
                    int unknow20 = binReader.ReadInt32();
                    string unknow21 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow22 = binReader.ReadInt32();
                    int unknow23 = binReader.ReadInt32();
                    int unknow24 = binReader.ReadInt32();
                    int unknow25 = binReader.ReadInt32();
                    string unknow26 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow27 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow28 = binReader.ReadInt32();
                    int unknow29 = binReader.ReadInt32();
                    int unknow30 = binReader.ReadInt32();
                    string unknow31 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow32 = binReader.ReadInt32();
                    int unknow33 = binReader.ReadInt32();
                    int unknow34 = binReader.ReadInt32();
                    string unknow35 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow36 = binReader.ReadInt32();
                    int unknow37 = binReader.ReadInt32();
                    int unknow38 = binReader.ReadInt32();
                    int unknow39 = binReader.ReadInt32();
                    int unknow40 = binReader.ReadInt32();
                    int unknow41 = binReader.ReadInt32();
                    int unknow42 = binReader.ReadInt32();
                    int unknow43 = binReader.ReadInt32();
                    int unknow44 = binReader.ReadInt32();
                    int unknow45 = binReader.ReadInt32();
                    int unknow46 = binReader.ReadInt32();
                    int unknow47 = binReader.ReadInt32();
                    int unknow48 = binReader.ReadInt32();
                    int unknow49 = binReader.ReadInt32();
                    int unknow50 = binReader.ReadInt32();
                    int unknow51 = binReader.ReadInt32();
                    int unknow52 = binReader.ReadInt32();
                    int unknow53 = binReader.ReadInt32();
                    int unknow54 = binReader.ReadInt32();
                    int unknow55 = binReader.ReadInt32();
                    int unknow56 = binReader.ReadInt32();
                    int unknow57 = binReader.ReadInt32();
                    int unknow58 = binReader.ReadInt32();
                    int unknow59 = binReader.ReadInt32();
                    string unknow60 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow61 = binReader.ReadInt32();
                    int unknow62 = binReader.ReadInt32();
                    int unknow63 = binReader.ReadInt32();
                    int unknow64 = binReader.ReadInt32();
                    int unknow65 = binReader.ReadInt32();
                    int unknow66 = binReader.ReadInt32();
                    int unknow67 = binReader.ReadInt32();
                    int unknow68 = binReader.ReadInt32();
                    int unknow69 = binReader.ReadInt32();
                    int unknow70 = binReader.ReadInt32();
                    int unknow71 = binReader.ReadInt32();
                    int unknow72 = binReader.ReadInt32();
                    int unknow73 = binReader.ReadInt32();
                    int unknow74 = binReader.ReadInt32();
                    int unknow75 = binReader.ReadInt32();
                    int unknow76 = binReader.ReadInt32();
                    int unknow77 = binReader.ReadInt32();
                    int unknow78 = binReader.ReadInt32();
                    int unknow79 = binReader.ReadInt32();
                    int unknow80 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknowText + "|" + unknow12 + "|" + unknow13 + "|" + unknow14 + "|" + unknow15 + "|" + unknow16 + "|" + unknow17 + "|" + unknow18 + "|" + unknow19 + "|" + unknow20 + "|" + unknow21 + "|" + unknow22 + "|" + unknow23 + "|" + unknow24 + "|" + unknow25 + "|" + unknow26 + "|" + unknow27 + "|" + unknow28 + "|" + unknow29 + "|" + unknow30 + "|" + unknow31 + "|" + unknow32 + "|" + unknow33 + "|" + unknow34 + "|" + unknow35 + "|" + unknow36 + "|" + unknow37 + "|" + unknow38 + "|" + unknow39 + "|" + unknow40 + "|" + unknow41 + "|" + unknow42 + "|" + unknow43 + "|" + unknow44 + "|" + unknow45 + "|" + unknow46 + "|" + unknow47 + "|" + unknow48 + "|" + unknow49 + "|" + unknow50 + "|" + unknow51 + "|" + unknow52 + "|" + unknow53 + "|" + unknow54 + "|" + unknow55 + "|" + unknow56 + "|" + unknow57 + "|" + unknow58 + "|" + unknow59 + "|" + unknow60 + "|" + unknow61 + "|" + unknow62 + "|" + unknow63 + "|" + unknow64 + "|" + unknow65 + "|" + unknow66 + "|" + unknow67 + "|" + unknow68 + "|" + unknow69 + "|" + unknow70 + "|" + unknow71 + "|" + unknow72 + "|" + unknow73 + "|" + unknow74 + "|" + unknow75 + "|" + unknow76 + "|" + unknow77 + "|" + unknow78 + "|" + unknow79 + "|" + unknow80 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadHBOVerFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|1|");

            int TotalEntry = 0;
            if (VersionNumber == "051220")
            {
                do
                {
                    string VersionHBONumber = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));


                    ListRow.Add(VersionHBONumber + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadHairForPlayerFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|1|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    uint unknow1 = binReader.ReadUInt32();


                    ListRow.Add(unknow1 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadGreetingIngDialFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");

            int TotalEntry = 0;
            if (VersionNumber == "060601")
            {
                do
                {
                    uint unknow1 = binReader.ReadUInt32();
                    string text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));


                    ListRow.Add(unknow1 + "|" + text1 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadFormulaParametersFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");

            int TotalEntry = 0;
            if (VersionNumber == "061213")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    string text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));


                    ListRow.Add(unknow1 + "|" + text1 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadFamilyFunctionControlFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);

            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|10|");

            int TotalEntry = 0;
            if (VersionNumber == "060929")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    int unknow8 = binReader.ReadInt32();
                    int unknow9 = binReader.ReadInt32();
                    int unknow10 = binReader.ReadInt32();


                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadFaceForPlayerFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|45|");

            int TotalEntry = 0;
            if (VersionNumber == "050620")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    int unknow8 = binReader.ReadInt32();
                    int unknow9 = binReader.ReadInt32();
                    int unknow10 = binReader.ReadInt32();
                    int unknow11 = binReader.ReadInt32();
                    int unknow12 = binReader.ReadInt32();
                    int unknow13 = binReader.ReadInt32();
                    int unknow14 = binReader.ReadInt32();
                    int unknow15 = binReader.ReadInt32();
                    int unknow16 = binReader.ReadInt32();
                    int unknow17 = binReader.ReadInt32();
                    int unknow18 = binReader.ReadInt32();
                    int unknow19 = binReader.ReadInt32();
                    int unknow20 = binReader.ReadInt32();
                    int unknow21 = binReader.ReadInt32();
                    int unknow22 = binReader.ReadInt32();
                    int unknow23 = binReader.ReadInt32();
                    int unknow24 = binReader.ReadInt32();
                    int unknow25 = binReader.ReadInt32();
                    int unknow26 = binReader.ReadInt32();
                    int unknow27 = binReader.ReadInt32();
                    int unknow28 = binReader.ReadInt32();
                    int unknow29 = binReader.ReadInt32();
                    int unknow30 = binReader.ReadInt32();
                    int unknow31 = binReader.ReadInt32();
                    int unknow32 = binReader.ReadInt32();
                    int unknow33 = binReader.ReadInt32();
                    int unknow34 = binReader.ReadInt32();
                    int unknow35 = binReader.ReadInt32();
                    int unknow36 = binReader.ReadInt32();
                    int unknow37 = binReader.ReadInt32();
                    int unknow38 = binReader.ReadInt32();
                    int unknow39 = binReader.ReadInt32();
                    int unknow40 = binReader.ReadInt32();
                    int unknow41 = binReader.ReadInt32();
                    int unknow42 = binReader.ReadInt32();
                    int unknow43 = binReader.ReadInt32();
                    int unknow44 = binReader.ReadInt32();
                    int unknow45 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknow12 + "|" + unknow13 + "|" + unknow14 + "|" + unknow15 + "|" + unknow16 + "|" + unknow17 + "|" + unknow18 + "|" + unknow19 + "|" + unknow20 + "|" + unknow21 + "|" + unknow22 + "|" + unknow23 + "|" + unknow24 + "|" + unknow25 + "|" + unknow26 + "|" + unknow27 + "|" + unknow28 + "|" + unknow29 + "|" + unknow30 + "|" + unknow31 + "|" + unknow32 + "|" + unknow33 + "|" + unknow34 + "|" + unknow35 + "|" + unknow36 + "|" + unknow37 + "|" + unknow38 + "|" + unknow39 + "|" + unknow40 + "|" + unknow41 + "|" + unknow42 + "|" + unknow43 + "|" + unknow44 + "|" + unknow45 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadEventAreaFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|9|");

            int TotalEntry = 0;
            if (VersionNumber == "061201")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    float unknow3 = binReader.ReadSingle();
                    float unknow4 = binReader.ReadSingle();
                    float unknow5 = binReader.ReadSingle();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    string unknow8 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow9 = binReader.ReadInt32();


                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadEnvSoundFxFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|13|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    float unknow5 = binReader.ReadSingle();
                    float unknow6 = binReader.ReadSingle();
                    float unknow7 = binReader.ReadSingle();
                    float unknow8 = binReader.ReadSingle();
                    float unknow9 = binReader.ReadSingle();
                    float unknow10 = binReader.ReadSingle();
                    int unknow11 = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + Text1 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }


        private List<string> ReadEmotionFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|13|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int unknow1     = binReader.ReadInt32();
                    string Text1    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text2    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    int unknow2     = binReader.ReadInt32();
                    string Text3    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    int unknow3     = binReader.ReadInt32();
                    int unknow4     = binReader.ReadInt32();
                    int unknow5     = binReader.ReadInt32();
                    int unknow6     = binReader.ReadInt32();
                    int unknow7     = binReader.ReadInt32();
                    int unknow8     = binReader.ReadInt32();
                    int unknow9     = binReader.ReadInt32();
                    string Text4 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(unknow1 + "|" + Text1 + "|" + Text2 + "|" + unknow2 + "|" + Text3 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + Text4 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadEffectFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|19|");

            int TotalEntry = 0;
            if (VersionNumber == "060601")
            {
                do
                {
                    int unknow1     = binReader.ReadInt32();
                    string Text1    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text2    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text3    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow2     = binReader.ReadInt32();
                    int unknow3     = binReader.ReadInt32();
                    int unknow4     = binReader.ReadInt32();

                    string Text4    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow5     = binReader.ReadInt32();
                    int unknow6     = binReader.ReadInt32();

                    string Text5    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow7     = binReader.ReadInt32();
                    string Text6    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text7    = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    int unknow8     = binReader.ReadInt32();
                    int unknow9     = binReader.ReadInt32();
                    int unknow10    = binReader.ReadInt32();
                    int unknow11    = binReader.ReadInt32();
                    int unknow12    = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + Text1 + "|" + Text2 + "|" + Text3 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + Text4 + "|" + unknow5 + "|" + unknow6 + "|" + Text5 + "|" + unknow7 + "|" + Text6 + "|" + Text7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknow12 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadCueMessageFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|3|");

            int TotalEntry = 0;
            if (VersionNumber == "060804")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadWeaponTypeFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|17|");

            int TotalEntry = 0;
            if (VersionNumber == "060601")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    int unknow8 = binReader.ReadInt32();
                    int unknow9 = binReader.ReadInt32();
                    int unknow10 = binReader.ReadInt32();
                    int unknow11 = binReader.ReadInt32();
                    int unknow12 = binReader.ReadInt32();
                    int unknow13 = binReader.ReadInt32();
                    int unknow14 = binReader.ReadInt32();
                    int unknow15 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + Text1 + "|" + Text2 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknow12 + "|" + unknow13 + "|" + unknow14 + "|" + unknow15 + "|");

                   TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadCompoundFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|20|");

            int TotalEntry = 0;
            if (VersionNumber == "070416")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    int unknow8 = binReader.ReadInt32();
                    int unknow9 = binReader.ReadInt32();
                    int unknow10 = binReader.ReadInt32();
                    int unknow11 = binReader.ReadInt32();
                    int unknow12 = binReader.ReadInt32();
                    int unknow13 = binReader.ReadInt32();
                    int unknow14 = binReader.ReadInt32();
                    int unknow15 = binReader.ReadInt32();
                    int unknow16 = binReader.ReadInt32();
                    int unknow17 = binReader.ReadInt32();
                    int unknow18 = binReader.ReadInt32();
                    int unknow19 = binReader.ReadInt32();
                    int unknow20 = binReader.ReadInt32();


                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknow12 + "|" + unknow13 + "|" + unknow14 + "|" + unknow15 + "|" + unknow16 + "|" + unknow17 + "|" + unknow18 + "|" + unknow19 + "|" + unknow20 + "|");

                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadColorFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|5|");

            int TotalEntry = 0;
            if (VersionNumber == "060628")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadClassSpellMasterFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");

            int TotalEntry = 0;
            if (VersionNumber == "060601")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadChannelConfigFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|6|");

            int TotalEntry = 0;
            if (VersionNumber == "051124")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();


                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadBornAreaFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|3|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + Text1 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadArmorTypeFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|5|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + Text1 + "|" + Text2 + "|" + unknow2 + "|" + unknow3 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadShopMenuFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6));

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|4|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadClassFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); 

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|3|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID + "|" + Text1 + "|" + Text2 + " |");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }


        private List<string> ReadShopFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|4|");

            int TotalEntry = 0;
            if (VersionNumber == "070118")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID + "|" + unknow1 + "|" + unknow2 + "|" + Text1 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadTextureIDFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32();
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID + "|" +  Text1 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadBgmFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|4|");

            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    int unknow1 = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string Text2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID + "|" + unknow1 + "|" + Text1 + "|" + Text2 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadCommonEffectFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|4|");

            int TotalEntry = 0;
            if (VersionNumber == "060607")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow1 = binReader.ReadInt32();
                    string Text2 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID + "|" + Text1 + "|" + unknow1 + "|" + Text2 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadTextIDFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|3|");


            //0x20 = 32
            int TotalEntry = 0;
            if (VersionNumber == "051122")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    string TextID = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow1 = binReader.ReadInt32();

                    ListRow.Add(IndexID + "|" + TextID + "|" + unknow1 + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadParticleIDFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");


            //0x20 = 32
            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    string ParticleName = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID + "|" + ParticleName + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadGatewayAreaFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|18|");


            //sub_5ED420 byte ?

            //0x20 = 32
            int TotalEntry = 0;
            if (VersionNumber == "050801")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    float unknow4 = binReader.ReadSingle();
                    float unknow5 = binReader.ReadSingle();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    string unknow8 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow9 = binReader.ReadInt32();
                    int unknow10 = binReader.ReadInt32();
                    float unknow11 = binReader.ReadSingle();
                    float unknow12 = binReader.ReadSingle();
                    int unknow13 = binReader.ReadInt32();
                    int unknow14 = binReader.ReadInt32();
                    string unknow15 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow16 = binReader.ReadInt32();
                    string unknow17 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    string unknow18 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknow12 + "|" + unknow13 + "|" + unknow14 + "|" + unknow15 + "|" + unknow16 + "|" + unknow17 + "|" + unknow18 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadSoundIDFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");


            //0x20 = 32
            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    string SoundName = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID + "|" + SoundName + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadAnimationDependFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|29|");

            // 0xC
            int TotalEntry = 0;
            if (VersionNumber == "060601")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    string Text1 = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    int unknow4 = binReader.ReadInt32();
                    int unknow5 = binReader.ReadInt32();
                    int unknow6 = binReader.ReadInt32();
                    int unknow7 = binReader.ReadInt32();
                    int unknow8 = binReader.ReadInt32();
                    int unknow9 = binReader.ReadInt32();
                    int unknow10 = binReader.ReadInt32();
                    int unknow11 = binReader.ReadInt32();
                    int unknow12 = binReader.ReadInt32();
                    int unknow13 = binReader.ReadInt32();
                    int unknow14 = binReader.ReadInt32();
                    int unknow15 = binReader.ReadInt32();
                    int unknow16 = binReader.ReadInt32();
                    int unknow17 = binReader.ReadInt32();
                    int unknow18 = binReader.ReadInt32();
                    int unknow19 = binReader.ReadInt32();
                    int unknow20 = binReader.ReadInt32();
                    int unknow21 = binReader.ReadInt32();
                    int unknow22 = binReader.ReadInt32();
                    int unknow23 = binReader.ReadInt32();
                    int unknow24 = binReader.ReadInt32();
                    int unknow25 = binReader.ReadInt32();
                    int unknow26 = binReader.ReadInt32();
                    int unknow27 = binReader.ReadInt32();
                    int unknow28 = binReader.ReadInt32();



                    ListRow.Add(unknow1 + "|" + Text1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|" + unknow7 + "|" + unknow8 + "|" + unknow9 + "|" + unknow10 + "|" + unknow11 + "|" + unknow12 + "|" + unknow13 + "|" + unknow14 + "|" + unknow15 + "|" + unknow16 + "|" + unknow17 + "|" + unknow18 + "|" + unknow19 + "|" + unknow20 + "|" + unknow21 + "|" + unknow22 + "|" + unknow23 + "|" + unknow24 + "|" + unknow25 + "|" + unknow26 + "|" + unknow27 + "|" + unknow28 + "|" );
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadAnimationFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            if (VersionNumber == "060601")
                ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|6|");
            else if (VersionNumber == "050425")
                ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|3|");

            // 0xC
            int TotalEntry = 0;
            if (VersionNumber == "050425" || VersionNumber == "060601")
            {
                do
                {
                    int unknow1 = binReader.ReadInt32();
                    int unknow2 = binReader.ReadInt32();
                    int unknow3 = binReader.ReadInt32();
                    if (VersionNumber == "060601")
                    {
                        int unknow4 = binReader.ReadInt32();
                        float unknow5 = binReader.ReadSingle();
                        float unknow6 = binReader.ReadSingle();
                        ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|" + unknow4 + "|" + unknow5 + "|" + unknow6 + "|");
                    }
                    else if (VersionNumber == "050425")
                    {
                        ListRow.Add(unknow1 + "|" + unknow2 + "|" + unknow3 + "|");
                    }

                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadLevelFile(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x64 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");

            // 0x8
            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int LevelNumber = binReader.ReadInt32();
                    int ExpNumber = binReader.ReadInt32();

                    ListRow.Add(LevelNumber + "|" + ExpNumber + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }

        private List<string> ReadKfmIndex(string fileName, string versionNumber)
        {
            List<string> ListRow = new List<string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open), big5);


            int NumberOfEntry = binReader.ReadInt32(); // 0x8E 0x00 0x00 0x00
            ushort Key = binReader.ReadUInt16(); // 0x06 0x00
            string VersionNumber = big5.GetString(binReader.ReadBytes(6)); // 050425

            ListRow.Add(NumberOfEntry + "|" + Key + "|" + VersionNumber + "|2|");

            
            //0x20 = 32
            int TotalEntry = 0;
            if (VersionNumber == "050425")
            {
                do
                {
                    int IndexID = binReader.ReadInt32();
                    string IndexName = big5.GetString(binReader.ReadBytes(binReader.ReadUInt16()));

                    ListRow.Add(IndexID +  "|" + IndexName + "|");
                    TotalEntry++;
                }
                while (TotalEntry < NumberOfEntry);
            }

            return ListRow;
        }
    }
}
