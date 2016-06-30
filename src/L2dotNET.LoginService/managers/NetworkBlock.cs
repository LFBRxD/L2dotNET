﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using L2dotNET.Utility;

namespace L2dotNET.LoginService.Managers
{
    sealed class NetworkBlock
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NetworkBlock));

        private static volatile NetworkBlock instance;
        private static readonly object syncRoot = new object();

        public static NetworkBlock Instance
        {
            get
            {
                if (instance == null)
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new NetworkBlock();
                    }

                return instance;
            }
        }

        private readonly List<NB_interface> blocks = new List<NB_interface>();

        public NetworkBlock()
        {
            using (StreamReader reader = new StreamReader(new FileInfo(@"sq\blocks.txt").FullName))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine() ?? string.Empty;
                    if (line.Length == 0)
                        continue;

                    if (line.StartsWithIgnoreCase("//"))
                        continue;

                    if (line.StartsWithIgnoreCase("d"))
                    {
                        NB_interface i = new NB_interface();
                        i.directIp = line.Split(' ')[1];
                        i.forever = line.Split(' ')[2].EqualsIgnoreCase("0");
                        blocks.Add(i);
                    }
                    else if (line.StartsWithIgnoreCase("m"))
                    {
                        NB_interface i = new NB_interface();
                        i.mask = line.Split(' ')[1];
                        i.forever = line.Split(' ')[2].EqualsIgnoreCase("0");
                        blocks.Add(i);
                    }
                }
            }

            log.Info($"NetworkBlock: {blocks.Count} blocks.");
        }

        public bool Allowed(string ip)
        {
            if (blocks.Count == 0)
                return true;

            foreach (NB_interface nbi in blocks)
            {
                if (nbi.directIp != null)
                    if (nbi.directIp.Equals(ip))
                    {
                        if (nbi.forever)
                            return false;

                        if (nbi.timeEnd.CompareTo(DateTime.Now) == 1)
                            return false;
                    }

                if (nbi.mask != null)
                {
                    string[] a = ip.Split('.'),
                             b = nbi.mask.Split('.');
                    bool[] d = new bool[4];
                    for (byte c = 0; c < 4; c++)
                    {
                        d[c] = false;

                        if (b[c] == "*")
                            d[c] = true;
                        else if (b[c] == a[c])
                            d[c] = true;
                        else if (b[c].Contains("/"))
                        {
                            byte n = byte.Parse(b[c].Split('/')[0]),
                                 x = byte.Parse(b[c].Split('/')[1]);
                            byte t = byte.Parse(a[c]);
                            d[c] = (t >= n) && (t <= x);
                        }
                    }

                    byte cnt = (byte)d.Count(u => u);

                    if (cnt >= 4)
                        return false;
                }
            }

            return true;
        }
    }
}