using System;
using System.IO;
using CmlLib.Core;
using CmlLib.Core.ProcessBuilder;
using System.Windows.Forms;
using CmlLib.Core.Auth;

namespace Likuur_s_Minecraft_Launcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadMCVersionsAcync();
        }
        private async void LoadMCVersionsAcync()
        {
            var launcher = new MinecraftLauncher();
            var versions = await launcher.GetAllVersionsAsync();
            foreach ( var version in versions )
            {
                verionlist.Items.Add(version.Name);
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            string username = nickname.Text;
            string version = string.Empty;
            string mcpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "LMCLauncher");
            Directory.CreateDirectory(mcpath);
            var path = new MinecraftPath(mcpath);
            var launcher = new MinecraftLauncher(path);

            if (verionlist.SelectedIndex.ToString() != null && !string.IsNullOrEmpty(verionlist.SelectedIndex.ToString()))
            {
                version = verionlist.SelectedItem.ToString();
                await launcher.InstallAsync(version);
            }
            else
            {
                MessageBox.Show("Выберите версию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var minecraftProcess = await launcher.BuildProcessAsync(version, new MLaunchOption
            {
                Session = MSession.CreateOfflineSession(username),
                MaximumRamMb = (int)maxram.Value,
                GameLauncherName = "Likuur`s Minecraft Launcher",
                ServerIp = "mc.vimemc.net"
            });

            minecraftProcess.Start();
        }
    }
}