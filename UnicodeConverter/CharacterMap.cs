using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections;

namespace UnicodeConverter
{
    public static class CharacterMap
    {

        public static Hashtable ip2uc = new Hashtable();
        public static Hashtable cpi2u = new Hashtable();
        public static Hashtable cpu2i = new Hashtable();

        public static void initIp2ucCharacter()
        {
            ip2uc.Add(32, 32);
            ip2uc.Add(129, 1575);
            ip2uc.Add(130, 1576);
            ip2uc.Add(131, 1662);
            ip2uc.Add(132, 1578);
            ip2uc.Add(133, 1657);
            ip2uc.Add(134, 1579);
            ip2uc.Add(135, 1580);
            ip2uc.Add(136, 1670);
            ip2uc.Add(137, 1581);
            ip2uc.Add(138, 1582);
            ip2uc.Add(139, 1583);
            ip2uc.Add(140, 1672);
            //ڈ
            ip2uc.Add(141, 1584);
            ip2uc.Add(142, 1585);
            ip2uc.Add(143, 1681);
            ip2uc.Add(144, 1586);
            ip2uc.Add(145, 1688);
            ip2uc.Add(146, 1587);
            ip2uc.Add(147, 1588);
            ip2uc.Add(148, 1589);
            ip2uc.Add(149, 1590);
            ip2uc.Add(150, 1591);
            // old 1591
            ip2uc.Add(151, 1592);
            ip2uc.Add(152, 1593);
            ip2uc.Add(153, 1594);
            ip2uc.Add(154, 1601);
            ip2uc.Add(155, 1602);
            ip2uc.Add(156, 1705);
            ip2uc.Add(157, 1711);
            ip2uc.Add(158, 1604);
            ip2uc.Add(159, 1605);
            ip2uc.Add(160, 1606);
            ip2uc.Add(161, 1722);
            ip2uc.Add(162, 1608);
            ip2uc.Add(163, 1569);
            // // ء if after space then ok else change with 191 // Done 
            ip2uc.Add(164, 1740);
            //ی  
            ip2uc.Add(165, 1746);
            // bari ya
            ip2uc.Add(166, 1729);
            //  old 1729
            ip2uc.Add(167, 1726);
            ip2uc.Add(168, 1613);
            // tanven niche
            ip2uc.Add(169, 1600);
            ip2uc.Add(170, 1616);
            // zair
            ip2uc.Add(171, 1614);
            // zabar

            ip2uc.Add(172, 1615);
            // paish
            ip2uc.Add(173, 1617);
            // tashdeed
            ip2uc.Add(174, 1553);
            ip2uc.Add(176, 1622);
            // kari zeer
            ip2uc.Add(177, 1618);
            // jazam   new 1761 old 1618 
            ip2uc.Add(179, 1619);
            ip2uc.Add(180, 1618);
            // /// new 1618 old 1624  value ? not 100%  
            ip2uc.Add(181, 1612);
            //
            ip2uc.Add(182, 1572);
            ip2uc.Add(183, 1574);
            //ئ
            ip2uc.Add(184, 1610);
            //ي can't change the position add space after 184
            ip2uc.Add(185, 1731);
            ip2uc.Add(189, 1648);
            // kari zabar
            ip2uc.Add(190, 1623);
            // ulta paish
            ip2uc.Add(191, 1620);
            // hamza above '///ئ hamza Value(1574) ya * add after hamza, Remove last charcter which will be ya ی  
            ip2uc.Add(199, 1611);
            // tanveen above
            ip2uc.Add(200, 1570);
            ip2uc.Add(201, 1571);
            ip2uc.Add(202, 1573);
            ip2uc.Add(203, 65010);
            ip2uc.Add(207, 1556);
            ip2uc.Add(208, 1776);
            // 0
            ip2uc.Add(209, 1777);
            // 1
            ip2uc.Add(210, 1778);
            ip2uc.Add(211, 1779);
            ip2uc.Add(212, 1780);
            ip2uc.Add(213, 1781);
            ip2uc.Add(214, 1782);
            ip2uc.Add(215, 1783);
            ip2uc.Add(216, 1784);
            ip2uc.Add(217, 1785);
            ip2uc.Add(218, 33);
            ip2uc.Add(219, 64830);
            ip2uc.Add(220, 64831);
            ip2uc.Add(222, 37);
            ip2uc.Add(223, 47);
            ip2uc.Add(224, 8230);
            // 3 spaces and dot
            ip2uc.Add(225, 41);
            ip2uc.Add(226, 40);
            ip2uc.Add(228, 43);
            ip2uc.Add(230, 1555);
            ip2uc.Add(231, 1554);
            ip2uc.Add(232, 1645);
            ip2uc.Add(233, 58);
            ip2uc.Add(234, 1563);
            ip2uc.Add(235, 215);
            ip2uc.Add(236, 61);
            ip2uc.Add(237, 1548);
            ip2uc.Add(238, 1567);
            ip2uc.Add(239, 247);
            ip2uc.Add(241, 1549);
            // OLD 33
            ip2uc.Add(242, 1538);
            ip2uc.Add(243, 1748);
            // 243 & 245 have same  value
            ip2uc.Add(245, 45);
            ip2uc.Add(246, 65018);
            // (PBUH)
            ip2uc.Add(247, 1537);
            ip2uc.Add(248, 1552);
            ip2uc.Add(249, 44);
            ip2uc.Add(250, 93);
            ip2uc.Add(251, 91);
            ip2uc.Add(252, 8228);
            // .
            ip2uc.Add(253, 8217);
            // ’ hamza in web form
            ip2uc.Add(254, 8216);

        }

        public static void init_cpinpage2unicode()
        {
            cpi2u.Add(32, 32);
            cpi2u.Add(129, 1575);
            cpi2u.Add(8218, 1576);
            cpi2u.Add(402, 1662);
            cpi2u.Add(8222, 1578);
            cpi2u.Add(8230, 1657);
            cpi2u.Add(8224, 1579);
            cpi2u.Add(8225, 1580);
            cpi2u.Add(710, 1670);
            cpi2u.Add(8240, 1581);
            cpi2u.Add(352, 1582);
            cpi2u.Add(8249, 1583);
            cpi2u.Add(338, 1672);
            cpi2u.Add(141, 1584);
            cpi2u.Add(381, 1585);
            cpi2u.Add(143, 1681);
            cpi2u.Add(144, 1586);
            cpi2u.Add(8216, 1688);
            cpi2u.Add(8217, 1587);
            cpi2u.Add(8220, 1588);
            cpi2u.Add(8221, 1589);
            cpi2u.Add(8226, 1590);
            cpi2u.Add(8211, 1591);
            cpi2u.Add(8212, 1592);
            cpi2u.Add(732, 1593);
            cpi2u.Add(8482, 1594);
            cpi2u.Add(353, 1601);
            cpi2u.Add(8250, 1602);
            cpi2u.Add(339, 1705);
            cpi2u.Add(157, 1711);
            cpi2u.Add(382, 1604);
            cpi2u.Add(376, 1605);
            cpi2u.Add(160, 1606);
            cpi2u.Add(161, 1722);
            cpi2u.Add(162, 1608);
            cpi2u.Add(163, 1569);
            cpi2u.Add(164, 1740);
            cpi2u.Add(165, 1746);
            cpi2u.Add(166, 1729);
            cpi2u.Add(167, 1726);
            cpi2u.Add(168, 1613);
            cpi2u.Add(169, 1600);
            cpi2u.Add(170, 1616);
            cpi2u.Add(171, 1614);
            cpi2u.Add(172, 1615);
            cpi2u.Add(173, 1617);
            cpi2u.Add(174, 1553);
            cpi2u.Add(176, 1622);
            cpi2u.Add(177, 1618);
            cpi2u.Add(179, 1619);
            cpi2u.Add(180, 1618);
            cpi2u.Add(181, 1612);
            cpi2u.Add(182, 1572);
            cpi2u.Add(183, 1574);
            cpi2u.Add(184, 1610);
            cpi2u.Add(185, 1731);
            cpi2u.Add(189, 1648);
            cpi2u.Add(190, 1623);
            cpi2u.Add(191, 1620);
            cpi2u.Add(199, 1611);
            cpi2u.Add(200, 1570);
            cpi2u.Add(201, 1571);
            cpi2u.Add(202, 1573);
            cpi2u.Add(203, 65010);
            cpi2u.Add(207, 1556);
            cpi2u.Add(208, 1776);
            cpi2u.Add(209, 1777);
            cpi2u.Add(210, 1778);
            cpi2u.Add(211, 1779);
            cpi2u.Add(212, 1780);
            cpi2u.Add(213, 1781);
            cpi2u.Add(214, 1782);
            cpi2u.Add(215, 1783);
            cpi2u.Add(216, 1784);
            cpi2u.Add(217, 1785);
            cpi2u.Add(218, 33);
            cpi2u.Add(219, 64830);
            cpi2u.Add(220, 64831);
            cpi2u.Add(222, 37);
            cpi2u.Add(223, 47);
            cpi2u.Add(224, 8230);
            cpi2u.Add(225, 41);
            cpi2u.Add(226, 40);
            cpi2u.Add(228, 43);
            cpi2u.Add(230, 1555);
            cpi2u.Add(231, 1554);
            cpi2u.Add(232, 1645);
            cpi2u.Add(233, 58);
            cpi2u.Add(234, 1563);
            cpi2u.Add(235, 215);
            cpi2u.Add(236, 61);
            cpi2u.Add(237, 1548);
            cpi2u.Add(238, 1567);
            cpi2u.Add(239, 247);
            cpi2u.Add(241, 1549);
            cpi2u.Add(242, 1538);
            cpi2u.Add(243, 1748);
            cpi2u.Add(245, 45);
            cpi2u.Add(246, 65018);
            cpi2u.Add(247, 1537);
            cpi2u.Add(248, 1552);
            cpi2u.Add(249, 44);
            cpi2u.Add(250, 93);
            cpi2u.Add(251, 91);
            cpi2u.Add(252, 8228);
            cpi2u.Add(253, 8217);
            cpi2u.Add(254, 8216);


        }

        public static void init_cpunicode2inpage()
        {
            cpu2i.Add(32, 32);
            cpu2i.Add(1575, 129);
            cpu2i.Add(1576, 8218);
            cpu2i.Add(1662, 402);
            cpu2i.Add(1578, 8222);
            cpu2i.Add(1657, 8230);
            cpu2i.Add(1579, 8224);
            cpu2i.Add(1580, 8225);
            cpu2i.Add(1670, 710);
            cpu2i.Add(1581, 8240);
            cpu2i.Add(1582, 352);
            cpu2i.Add(1583, 8249);
            cpu2i.Add(1672, 338);
            cpu2i.Add(1584, 141);
            cpu2i.Add(1585, 381);
            cpu2i.Add(1681, 143);
            cpu2i.Add(1586, 144);
            cpu2i.Add(1688, 8216);
            cpu2i.Add(1587, 8217);
            cpu2i.Add(1588, 8220);
            cpu2i.Add(1589, 8221);
            cpu2i.Add(1590, 8226);
            cpu2i.Add(1591, 8211);
            cpu2i.Add(1592, 8212);
            cpu2i.Add(1593, 732);
            cpu2i.Add(1594, 8482);
            cpu2i.Add(1601, 353);
            cpu2i.Add(1602, 8250);
            cpu2i.Add(1705, 339);
            cpu2i.Add(1711, 157);
            cpu2i.Add(1604, 382);
            cpu2i.Add(1605, 376);
            cpu2i.Add(1606, 160);
            cpu2i.Add(1722, 161);
            cpu2i.Add(1608, 162);
            cpu2i.Add(1569, 163);
            cpu2i.Add(1740, 164);
            cpu2i.Add(1746, 165);
            cpu2i.Add(1729, 166);
            cpu2i.Add(1726, 167);
            cpu2i.Add(1613, 168);
            cpu2i.Add(1600, 169);
            cpu2i.Add(1616, 170);
            cpu2i.Add(1614, 171);
            cpu2i.Add(1615, 172);
            cpu2i.Add(1617, 173);
            cpu2i.Add(1553, 174);
            cpu2i.Add(1622, 176);
            cpu2i.Add(1761, 177);
            cpu2i.Add(1619, 179);
            cpu2i.Add(1618, 177);
            cpu2i.Add(1612, 181);
            cpu2i.Add(1572, 182);
            cpu2i.Add(1574, 183);
            cpu2i.Add(1610, 184);
            cpu2i.Add(1731, 185);
            cpu2i.Add(1648, 189);
            cpu2i.Add(1623, 190);
            cpu2i.Add(1620, 191);
            cpu2i.Add(1611, 199);
            cpu2i.Add(1570, 200);
            cpu2i.Add(1571, 201);
            cpu2i.Add(1573, 202);
            cpu2i.Add(65010, 203);
            cpu2i.Add(1556, 207);
            cpu2i.Add(1776, 208);
            //ginti 0
            cpu2i.Add(1777, 209);
            cpu2i.Add(1778, 210);
            cpu2i.Add(1779, 211);
            cpu2i.Add(1780, 212);
            cpu2i.Add(1781, 213);
            cpu2i.Add(1782, 214);
            cpu2i.Add(1783, 215);
            cpu2i.Add(1784, 216);
            cpu2i.Add(1785, 217);
            cpu2i.Add(33, 218);
            cpu2i.Add(64830, 219);
            cpu2i.Add(64831, 220);
            cpu2i.Add(37, 222);
            cpu2i.Add(47, 223);
            cpu2i.Add(8230, 224);
            cpu2i.Add(41, 225);
            cpu2i.Add(40, 226);
            cpu2i.Add(43, 228);
            cpu2i.Add(1555, 230);
            cpu2i.Add(1554, 231);
            cpu2i.Add(1645, 232);
            cpu2i.Add(58, 233);
            cpu2i.Add(1563, 234);
            cpu2i.Add(215, 235);
            cpu2i.Add(61, 236);
            cpu2i.Add(1548, 237);
            cpu2i.Add(1567, 238);
            cpu2i.Add(247, 239);
            cpu2i.Add(1549, 241);
            cpu2i.Add(1538, 242);
            cpu2i.Add(1748, 243);
            cpu2i.Add(45, 245);
            cpu2i.Add(65018, 246);
            cpu2i.Add(1537, 247);
            cpu2i.Add(1552, 248);
            cpu2i.Add(44, 249);
            cpu2i.Add(93, 250);
            cpu2i.Add(91, 251);
            cpu2i.Add(8228, 252);
            cpu2i.Add(8217, 253);
            cpu2i.Add(8216, 254);

            cpu2i.Add(1609, 164);
            // ى
            cpu2i.Add(1603, 339);
            // arbi ك
            cpu2i.Add(1607, 167);
            // arbi ها hah
            cpu2i.Add(1577, 185);
            // arbi ة
            cpu2i.Add(1706, 339);
            //arbi ڪ
            cpu2i.Add(1749, 166);
            // arbi ە
            cpu2i.Add(1632, 208);
            //ginti 0
            cpu2i.Add(1633, 209);
            // 1
            cpu2i.Add(1634, 210);
            // 2
            cpu2i.Add(1635, 211);
            cpu2i.Add(1636, 212);
            cpu2i.Add(1637, 213);
            cpu2i.Add(1638, 214);
            cpu2i.Add(1639, 215);
            cpu2i.Add(1640, 216);
            cpu2i.Add(1641, 217);
            cpu2i.Add(1642, 222);
            //%
            //cpu2i.Add(1728,)      'arbi  ء+ہ  ۀ
            //cpu2i.Add(1629,) ' value not found

        }

        public static string ChangePositon(string strVal)
        {
            string rtnVal = "";
            char[] my_char = strVal.ToCharArray();
            Int16 loop_value = (Int16) (my_char.Length - 1);

            while (!(loop_value == -1))
            {
                rtnVal += my_char[loop_value];
                loop_value += -1;
            }

            return rtnVal;
        }

        public static int FindStartPosition(byte[] bData)
        {

            for (int i = 0; i <= bData.Length - 1; i++)
            {
                if (bData[i] == 1 & bData[i + 4] == 13)
                {
                    string tempTest = "";
                    for (int t = 0; t <= 9; t += 1)
                    {
                        tempTest += bData[i + t].ToString();
                    }
                    if ("10001300000" == tempTest)
                    {
                        return i + 14;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }

            return -1;
        }

        public static int FindEndPosition(byte[] bData, int start)
        {
            for (int i = start; i <= bData.Length - 1; i++)
            {
                if (bData[i + 6] == 255)
                {
                    string tempTest = "";
                    for (int t = 0; t <= 9; t += 1)
                    {
                        tempTest += bData[i + t].ToString();
                    }
                    if ("1300000255255255255" == tempTest)
                    {
                        return i;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }

            return -1;
        }

        public static bool PbFindHamzaPosition(byte[] my_binaryData, Int32 i)
        {
            bool functionReturnValue = false;

            if (my_binaryData[i + 3] != 32 & !(my_binaryData[i + 2] == 13))
            {

                if (( my_binaryData[i + 3] == 170 | my_binaryData[i + 3] == 171 | my_binaryData[i + 3] == 172 | my_binaryData[i + 3] == 176 
                    | my_binaryData[i + 3] == 177                                                    
                    | my_binaryData[i + 3] == 168 | my_binaryData[i + 3] == 181 | my_binaryData[i + 3] == 173 | my_binaryData[i + 3] == 180 
                    | my_binaryData[i + 3] == 179 | my_binaryData[i + 3] == 169))              
                {                                                      

                    if (!(my_binaryData[i + 4] == 13 | my_binaryData[i + 5] == 32))
                    {
                        return true;
                        return functionReturnValue;

                    }
                    else
                    {
                        return false;
                        return functionReturnValue;
                    }
                }
                else if ((186 < my_binaryData[i + 3] & my_binaryData[i + 3] < 255))
                {
                    return false;
                    return functionReturnValue;
                }
                else
                {
                    return true;
                    return functionReturnValue;
                }
            }
            else
            {
                return false;
                return functionReturnValue;
            }
            return functionReturnValue;
        }

        public static string RevDig(string Digets)
        {
            string[] temArray = Digets.Split(' ');
            string temDigits = "";
            temArray = Digets.Split(' ');

            for (int i = temArray.Length - 1; i >= 0; i += -1)
            {
                temDigits += temArray[i] + " ";
            }
            return temDigits.Trim();
        }

    }

}