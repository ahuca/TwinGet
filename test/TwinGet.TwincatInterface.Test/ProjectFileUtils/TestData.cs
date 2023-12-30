namespace TwinGet.TwincatInterface.Test.ProjectFileUtils;

internal static class TestData
{

    public const string SolutionFileContent = """
        Microsoft Visual Studio Solution File, Format Version 12.00
        # Visual Studio 15
        VisualStudioVersion = 15.0.33403.129
        MinimumVisualStudioVersion = 10.0.40219.1
        Project("{B1E792BE-AA5F-4E3C-8C82-674BF9C0715B}") = "TestTwincatProject1", "TestTwincatProject1\TestTwincatProject1.tsproj", "{AF0AA87D-6A50-4129-B38E-8931819C4FEB}"
        EndProject
        Project("{B1E792BE-AA5F-4E3C-8C82-674BF9C0715B}") = "TestTwincatProject2", "TestTwincatProject2\TestTwincatProject2.tsproj", "{CD779436-E0BF-4C3D-9866-879794D3B6A4}"
        EndProject
        Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "TestNonTwincatProject1", "TestNonTwincatProject1\TestNonTwincatProject1.csproj", "{7A6348EF-DB45-4990-BEA5-70FA6567A01B}"
        EndProject
        Project("{DFBE7525-6864-4E62-8B2E-D530D69D9D96}") = "TestTwincatProject3", "TestTwincatProject3\TestTwincatProject3.tspproj", "{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}"
        EndProject
        Global
        	GlobalSection(SolutionConfigurationPlatforms) = preSolution
        		Debug|Any CPU = Debug|Any CPU
        		Debug|TwinCAT CE7 (ARMV7) = Debug|TwinCAT CE7 (ARMV7)
        		Debug|TwinCAT OS (ARMT2) = Debug|TwinCAT OS (ARMT2)
        		Debug|TwinCAT RT (x64) = Debug|TwinCAT RT (x64)
        		Debug|TwinCAT RT (x86) = Debug|TwinCAT RT (x86)
        		Release|Any CPU = Release|Any CPU
        		Release|TwinCAT CE7 (ARMV7) = Release|TwinCAT CE7 (ARMV7)
        		Release|TwinCAT OS (ARMT2) = Release|TwinCAT OS (ARMT2)
        		Release|TwinCAT RT (x64) = Release|TwinCAT RT (x64)
        		Release|TwinCAT RT (x86) = Release|TwinCAT RT (x86)
        	EndGlobalSection
        	GlobalSection(ProjectConfigurationPlatforms) = postSolution
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|Any CPU.ActiveCfg = Debug|TwinCAT RT (x86)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT CE7 (ARMV7).ActiveCfg = Debug|TwinCAT CE7 (ARMV7)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT CE7 (ARMV7).Build.0 = Debug|TwinCAT CE7 (ARMV7)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT OS (ARMT2).ActiveCfg = Debug|TwinCAT OS (ARMT2)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT OS (ARMT2).Build.0 = Debug|TwinCAT OS (ARMT2)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT RT (x64).ActiveCfg = Debug|TwinCAT RT (x64)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT RT (x64).Build.0 = Debug|TwinCAT RT (x64)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT RT (x86).ActiveCfg = Debug|TwinCAT RT (x86)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Debug|TwinCAT RT (x86).Build.0 = Debug|TwinCAT RT (x86)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|Any CPU.ActiveCfg = Release|TwinCAT RT (x86)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT CE7 (ARMV7).ActiveCfg = Release|TwinCAT CE7 (ARMV7)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT CE7 (ARMV7).Build.0 = Release|TwinCAT CE7 (ARMV7)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT OS (ARMT2).ActiveCfg = Release|TwinCAT OS (ARMT2)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT OS (ARMT2).Build.0 = Release|TwinCAT OS (ARMT2)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT RT (x64).ActiveCfg = Release|TwinCAT RT (x64)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT RT (x64).Build.0 = Release|TwinCAT RT (x64)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT RT (x86).ActiveCfg = Release|TwinCAT RT (x86)
        		{AF0AA87D-6A50-4129-B38E-8931819C4FEB}.Release|TwinCAT RT (x86).Build.0 = Release|TwinCAT RT (x86)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|Any CPU.ActiveCfg = Debug|TwinCAT RT (x86)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT CE7 (ARMV7).ActiveCfg = Debug|TwinCAT CE7 (ARMV7)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT CE7 (ARMV7).Build.0 = Debug|TwinCAT CE7 (ARMV7)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT OS (ARMT2).ActiveCfg = Debug|TwinCAT OS (ARMT2)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT OS (ARMT2).Build.0 = Debug|TwinCAT OS (ARMT2)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT RT (x64).ActiveCfg = Debug|TwinCAT RT (x64)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT RT (x64).Build.0 = Debug|TwinCAT RT (x64)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT RT (x86).ActiveCfg = Debug|TwinCAT RT (x86)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Debug|TwinCAT RT (x86).Build.0 = Debug|TwinCAT RT (x86)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|Any CPU.ActiveCfg = Release|TwinCAT RT (x86)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT CE7 (ARMV7).ActiveCfg = Release|TwinCAT CE7 (ARMV7)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT CE7 (ARMV7).Build.0 = Release|TwinCAT CE7 (ARMV7)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT OS (ARMT2).ActiveCfg = Release|TwinCAT OS (ARMT2)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT OS (ARMT2).Build.0 = Release|TwinCAT OS (ARMT2)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT RT (x64).ActiveCfg = Release|TwinCAT RT (x64)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT RT (x64).Build.0 = Release|TwinCAT RT (x64)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT RT (x86).ActiveCfg = Release|TwinCAT RT (x86)
        		{CD779436-E0BF-4C3D-9866-879794D3B6A4}.Release|TwinCAT RT (x86).Build.0 = Release|TwinCAT RT (x86)
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|Any CPU.Build.0 = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT CE7 (ARMV7).ActiveCfg = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT CE7 (ARMV7).Build.0 = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT OS (ARMT2).ActiveCfg = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT OS (ARMT2).Build.0 = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT RT (x64).ActiveCfg = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT RT (x64).Build.0 = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT RT (x86).ActiveCfg = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Debug|TwinCAT RT (x86).Build.0 = Debug|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|Any CPU.ActiveCfg = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|Any CPU.Build.0 = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT CE7 (ARMV7).ActiveCfg = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT CE7 (ARMV7).Build.0 = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT OS (ARMT2).ActiveCfg = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT OS (ARMT2).Build.0 = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT RT (x64).ActiveCfg = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT RT (x64).Build.0 = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT RT (x86).ActiveCfg = Release|Any CPU
        		{7A6348EF-DB45-4990-BEA5-70FA6567A01B}.Release|TwinCAT RT (x86).Build.0 = Release|Any CPU
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|Any CPU.ActiveCfg = Debug|TwinCAT OS (ARMT2)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT CE7 (ARMV7).ActiveCfg = Debug|TwinCAT CE7 (ARMV7)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT CE7 (ARMV7).Build.0 = Debug|TwinCAT CE7 (ARMV7)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT OS (ARMT2).ActiveCfg = Debug|TwinCAT OS (ARMT2)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT OS (ARMT2).Build.0 = Debug|TwinCAT OS (ARMT2)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT RT (x64).ActiveCfg = Debug|TwinCAT RT (x64)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT RT (x64).Build.0 = Debug|TwinCAT RT (x64)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT RT (x86).ActiveCfg = Debug|TwinCAT RT (x86)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Debug|TwinCAT RT (x86).Build.0 = Debug|TwinCAT RT (x86)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|Any CPU.ActiveCfg = Release|TwinCAT OS (ARMT2)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT CE7 (ARMV7).ActiveCfg = Release|TwinCAT CE7 (ARMV7)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT CE7 (ARMV7).Build.0 = Release|TwinCAT CE7 (ARMV7)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT OS (ARMT2).ActiveCfg = Release|TwinCAT OS (ARMT2)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT OS (ARMT2).Build.0 = Release|TwinCAT OS (ARMT2)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT RT (x64).ActiveCfg = Release|TwinCAT RT (x64)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT RT (x64).Build.0 = Release|TwinCAT RT (x64)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT RT (x86).ActiveCfg = Release|TwinCAT RT (x86)
        		{6B5DCAB8-2334-4AAE-A500-4E3139EB5C7E}.Release|TwinCAT RT (x86).Build.0 = Release|TwinCAT RT (x86)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|Any CPU.ActiveCfg = Debug|TwinCAT RT (x86)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|Any CPU.Build.0 = Debug|TwinCAT RT (x86)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT CE7 (ARMV7).ActiveCfg = Debug|TwinCAT CE7 (ARMV7)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT CE7 (ARMV7).Build.0 = Debug|TwinCAT CE7 (ARMV7)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT OS (ARMT2).ActiveCfg = Debug|TwinCAT OS (ARMT2)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT OS (ARMT2).Build.0 = Debug|TwinCAT OS (ARMT2)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT RT (x64).ActiveCfg = Debug|TwinCAT RT (x64)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT RT (x64).Build.0 = Debug|TwinCAT RT (x64)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT RT (x86).ActiveCfg = Debug|TwinCAT RT (x86)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Debug|TwinCAT RT (x86).Build.0 = Debug|TwinCAT RT (x86)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|Any CPU.ActiveCfg = Release|TwinCAT RT (x86)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT CE7 (ARMV7).ActiveCfg = Release|TwinCAT CE7 (ARMV7)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT CE7 (ARMV7).Build.0 = Release|TwinCAT CE7 (ARMV7)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT OS (ARMT2).ActiveCfg = Release|TwinCAT OS (ARMT2)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT OS (ARMT2).Build.0 = Release|TwinCAT OS (ARMT2)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT RT (x64).ActiveCfg = Release|TwinCAT RT (x64)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT RT (x64).Build.0 = Release|TwinCAT RT (x64)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT RT (x86).ActiveCfg = Release|TwinCAT RT (x86)
        		{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}.Release|TwinCAT RT (x86).Build.0 = Release|TwinCAT RT (x86)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|Any CPU.ActiveCfg = Debug|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|Any CPU.Build.0 = Debug|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT CE7 (ARMV7).ActiveCfg = Debug|TwinCAT CE7 (ARMV7)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT CE7 (ARMV7).Build.0 = Debug|TwinCAT CE7 (ARMV7)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT OS (ARMT2).ActiveCfg = Debug|TwinCAT OS (ARMT2)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT OS (ARMT2).Build.0 = Debug|TwinCAT OS (ARMT2)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT RT (x64).ActiveCfg = Debug|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT RT (x64).Build.0 = Debug|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT RT (x86).ActiveCfg = Debug|TwinCAT RT (x86)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Debug|TwinCAT RT (x86).Build.0 = Debug|TwinCAT RT (x86)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|Any CPU.ActiveCfg = Release|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|Any CPU.Build.0 = Release|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT CE7 (ARMV7).ActiveCfg = Release|TwinCAT CE7 (ARMV7)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT CE7 (ARMV7).Build.0 = Release|TwinCAT CE7 (ARMV7)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT OS (ARMT2).ActiveCfg = Release|TwinCAT OS (ARMT2)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT OS (ARMT2).Build.0 = Release|TwinCAT OS (ARMT2)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT RT (x64).ActiveCfg = Release|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT RT (x64).Build.0 = Release|TwinCAT RT (x64)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT RT (x86).ActiveCfg = Release|TwinCAT RT (x86)
        		{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}.Release|TwinCAT RT (x86).Build.0 = Release|TwinCAT RT (x86)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|Any CPU.ActiveCfg = Debug|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|Any CPU.Build.0 = Debug|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT CE7 (ARMV7).ActiveCfg = Debug|TwinCAT CE7 (ARMV7)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT CE7 (ARMV7).Build.0 = Debug|TwinCAT CE7 (ARMV7)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT OS (ARMT2).ActiveCfg = Debug|TwinCAT OS (ARMT2)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT OS (ARMT2).Build.0 = Debug|TwinCAT OS (ARMT2)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT RT (x64).ActiveCfg = Debug|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT RT (x64).Build.0 = Debug|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT RT (x86).ActiveCfg = Debug|TwinCAT RT (x86)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Debug|TwinCAT RT (x86).Build.0 = Debug|TwinCAT RT (x86)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|Any CPU.ActiveCfg = Release|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|Any CPU.Build.0 = Release|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT CE7 (ARMV7).ActiveCfg = Release|TwinCAT CE7 (ARMV7)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT CE7 (ARMV7).Build.0 = Release|TwinCAT CE7 (ARMV7)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT OS (ARMT2).ActiveCfg = Release|TwinCAT OS (ARMT2)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT OS (ARMT2).Build.0 = Release|TwinCAT OS (ARMT2)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT RT (x64).ActiveCfg = Release|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT RT (x64).Build.0 = Release|TwinCAT RT (x64)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT RT (x86).ActiveCfg = Release|TwinCAT RT (x86)
        		{15A1AB25-0A54-4917-9522-8422C161DFF9}.Release|TwinCAT RT (x86).Build.0 = Release|TwinCAT RT (x86)
        	EndGlobalSection
        	GlobalSection(SolutionProperties) = preSolution
        		HideSolutionNode = FALSE
        	EndGlobalSection
        	GlobalSection(ExtensibilityGlobals) = postSolution
        		SolutionGuid = {DD06B855-CD6D-4014-BB8B-6B16769CEF3C}
        	EndGlobalSection
        EndGlobal

        """;


    public const string TwincatProjectGuid = "{AF0AA87D-6A50-4129-B38E-8931819C4FEB}";
    public const string TwincatFileContent = $$"""
        <?xml version="1.0"?>
        <TcSmProject xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="http://www.beckhoff.com/schemas/2012/07/TcSmProject" TcSmVersion="1.0" TcVersion="3.1.4024.53">
        	<Project ProjectGUID="{{TwincatProjectGuid}}" ShowHideConfigurations="#x3c7">
        		<System>
        			<Tasks>
        				<Task Id="3" Priority="20" CycleTime="100000" AmsPort="350" AdtTasks="true">
        					<Name>PlcTask</Name>
        				</Task>
        			</Tasks>
        		</System>
        		<Plc>
        			<Project GUID="{F423D9F4-0EF7-4885-8EC0-9A403A78EC70}" Name="TestPlcProject1" PrjFilePath="TestPlcProject1\TestPlcProject1.plcproj" TmcFilePath="TestPlcProject1\TestPlcProject1.tmc" ReloadTmc="true" AmsPort="851" FileArchiveSettings="#x000e" SymbolicMapping="true">
        				<Instance Id="#x08502000" TcSmClass="TComPlcObjDef" KeepUnrestoredLinks="2" TmcPath="TestPlcProject1\TestPlcProject1.tmc" TmcHash="{7B06BEE5-6CF9-9104-20B7-826712E47DEA}">
        					<Name>TestPlcProject1 Instance</Name>
        					<CLSID ClassFactory="TcPlc30">{08500001-0000-0000-F000-000000000064}</CLSID>
        					<Contexts>
        						<Context>
        							<Id>0</Id>
        							<Name>PlcTask</Name>
        							<ManualConfig>
        								<OTCID>#x02010030</OTCID>
        							</ManualConfig>
        							<Priority>20</Priority>
        							<CycleTime>10000000</CycleTime>
        						</Context>
        					</Contexts>
        					<TaskPouOids>
        						<TaskPouOid Prio="20" OTCID="#x08502001"/>
        					</TaskPouOids>
        				</Instance>
        			</Project>
        			<Project GUID="{A356EEB8-AAFC-4761-A0DD-0B8C83CA855D}" Name="TestPlcProject2" PrjFilePath="TestPlcProject2\TestPlcProject2.plcproj" TmcFilePath="TestPlcProject2\TestPlcProject2.tmc" ReloadTmc="true" AmsPort="852" FileArchiveSettings="#x000e" SymbolicMapping="true">
        				<Instance Id="#x08502040" TcSmClass="TComPlcObjDef" KeepUnrestoredLinks="2">
        					<Name>TestPlcProject2 Instance</Name>
        					<CLSID ClassFactory="TcPlc30">{08500001-0000-0000-F000-000000000064}</CLSID>
        					<Contexts>
        						<Context>
        							<Id>1</Id>
        							<Name>Default</Name>
        						</Context>
        					</Contexts>
        				</Instance>
        			</Project>
        		</Plc>
        	</Project>
        </TcSmProject>
        """;

    public const string PlcProjectGuid = "{f423d9f4-0ef7-4885-8ec0-9a403a78ec70}";

    public const string PlcFileContent = $$"""
        <?xml version="1.0" encoding="utf-8"?>
        <Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
          <PropertyGroup>
            <FileVersion>1.0.0.0</FileVersion>
            <SchemaVersion>2.0</SchemaVersion>
            <ProjectGuid>{{PlcProjectGuid}}</ProjectGuid>
            <SubObjectsSortedByName>True</SubObjectsSortedByName>
            <DownloadApplicationInfo>true</DownloadApplicationInfo>
            <WriteProductVersion>true</WriteProductVersion>
            <GenerateTpy>false</GenerateTpy>
            <Name>TestPlcProject1</Name>
            <ProgramVersion>3.1.4024.0</ProgramVersion>
            <Application>{da8cb013 - 01e2 - 4e76 - aad8 - c63cb197c3b9}</Application>
            <TypeSystem>{65a0cb05-45a0-424f-9370-5321fe656f5f}</TypeSystem>
            <Implicit_Task_Info>{d1e585ee - 3f7a-4fb2-acd6-080b80c26dc3}</Implicit_Task_Info>
            <Implicit_KindOfTask>{e3297877 - f97b - 44dc-8428-54f3df37edf6}</Implicit_KindOfTask>
            <Implicit_Jitter_Distribution>{74d54b3b-a48f-4f65-8df7-855cc9cd89e2}</Implicit_Jitter_Distribution>
            <LibraryReferences>{bd174e45 - 2719 - 4cbf-b50e-b235af23264e}</LibraryReferences>
            <Company>TwinGet</Company>
            <Released>false</Released>
            <Title>TwinGet.TestTwincatProject1.TestPlcProject1</Title>
            <ProjectVersion>0.1.0</ProjectVersion>
            <LibraryCategories>
              <LibraryCategory xmlns="">
                <Id>{736e1fb9-7997-4a5d-a19a-cfcf55bcd1a4}</Id>
                <Version>1.0.0.0</Version>
                <DefaultName>TwinGet</DefaultName>
              </LibraryCategory>
            </LibraryCategories>
            <SelectedLibraryCategories>
              <Id xmlns="">{736e1fb9-7997-4a5d-a19a-cfcf55bcd1a4}</Id>
            </SelectedLibraryCategories>
            <Author>Author</Author>
            <Description>Description</Description>
          </PropertyGroup>
          <ItemGroup>
            <Compile Include="PlcTask.TcTTO">
              <SubType>Code</SubType>
            </Compile>
            <Compile Include="POUs\MAIN.TcPOU">
              <SubType>Code</SubType>
            </Compile>
          </ItemGroup>
          <ItemGroup>
            <Folder Include="DUTs" />
            <Folder Include="GVLs" />
            <Folder Include="VISUs" />
            <Folder Include="POUs" />
          </ItemGroup>
          <ItemGroup>
            <PlaceholderReference Include="Tc2_Standard">
              <DefaultResolution>Tc2_Standard, * (Beckhoff Automation GmbH)</DefaultResolution>
              <Namespace>Tc2_Standard</Namespace>
            </PlaceholderReference>
            <PlaceholderReference Include="Tc2_System">
              <DefaultResolution>Tc2_System, * (Beckhoff Automation GmbH)</DefaultResolution>
              <Namespace>Tc2_System</Namespace>
            </PlaceholderReference>
            <PlaceholderReference Include="Tc3_Module">
              <DefaultResolution>Tc3_Module, * (Beckhoff Automation GmbH)</DefaultResolution>
              <Namespace>Tc3_Module</Namespace>
            </PlaceholderReference>
          </ItemGroup>
          <ProjectExtensions>
            <PlcProjectOptions>
              <XmlArchive>
                <Data>
                  <o xml:space="preserve" t="OptionKey">
              <v n="Name">"&lt;ProjectRoot&gt;"</v>
              <d n="SubKeys" t="Hashtable" ckt="String" cvt="OptionKey">
                <v>{40450F57-0AA3-4216-96F3-5444ECB29763}</v>
                <o>
                  <v n="Name">"{40450F57-0AA3-4216-96F3-5444ECB29763}"</v>
                  <d n="SubKeys" t="Hashtable" />
                  <d n="Values" t="Hashtable" ckt="String" cvt="String">
                    <v>ActiveVisuProfile</v>
                    <v>IR0whWr8bwfwBwAAiD2qpQAAAABVAgAA37x72QAAAAABAAAAAAAAAAEaUwB5AHMAdABlAG0ALgBTAHQAcgBpAG4AZwACTHsAZgA5ADUAYgBiADQAMgA2AC0ANQA1ADIANAAtADQAYgA0ADUALQA5ADQAMAAwAC0AZgBiADAAZgAyAGUANwA3AGUANQAxAGIAfQADCE4AYQBtAGUABDBUAHcAaQBuAEMAQQBUACAAMwAuADEAIABCAHUAaQBsAGQAIAA0ADAAMgA0AC4ANwAFFlAAcgBvAGYAaQBsAGUARABhAHQAYQAGTHsAMQA2AGUANQA1AGIANgAwAC0ANwAwADQAMwAtADQAYQA2ADMALQBiADYANQBiAC0ANgAxADQANwAxADMAOAA3ADgAZAA0ADIAfQAHEkwAaQBiAHIAYQByAGkAZQBzAAhMewAzAGIAZgBkADUANAA1ADkALQBiADAANwBmAC0ANABkADYAZQAtAGEAZQAxAGEALQBhADgAMwAzADUANgBhADUANQAxADQAMgB9AAlMewA5AGMAOQA1ADgAOQA2ADgALQAyAGMAOAA1AC0ANAAxAGIAYgAtADgAOAA3ADEALQA4ADkANQBmAGYAMQBmAGUAZABlADEAYQB9AAoOVgBlAHIAcwBpAG8AbgALBmkAbgB0AAwKVQBzAGEAZwBlAA0KVABpAHQAbABlAA4aVgBpAHMAdQBFAGwAZQBtAE0AZQB0AGUAcgAPDkMAbwBtAHAAYQBuAHkAEAxTAHkAcwB0AGUAbQARElYAaQBzAHUARQBsAGUAbQBzABIwVgBpAHMAdQBFAGwAZQBtAHMAUwBwAGUAYwBpAGEAbABDAG8AbgB0AHIAbwBsAHMAEyhWAGkAcwB1AEUAbABlAG0AcwBXAGkAbgBDAG8AbgB0AHIAbwBsAHMAFCRWAGkAcwB1AEUAbABlAG0AVABlAHgAdABFAGQAaQB0AG8AcgAVIlYAaQBzAHUATgBhAHQAaQB2AGUAQwBvAG4AdAByAG8AbAAWFHYAaQBzAHUAaQBuAHAAdQB0AHMAFwxzAHkAcwB0AGUAbQAYGFYAaQBzAHUARQBsAGUAbQBCAGEAcwBlABkmRABlAHYAUABsAGEAYwBlAGgAbwBsAGQAZQByAHMAVQBzAGUAZAAaCGIAbwBvAGwAGyJQAGwAdQBnAGkAbgBDAG8AbgBzAHQAcgBhAGkAbgB0AHMAHEx7ADQAMwBkADUAMgBiAGMAZQAtADkANAAyAGMALQA0ADQAZAA3AC0AOQBlADkANAAtADEAYgBmAGQAZgAzADEAMABlADYAMwBjAH0AHRxBAHQATABlAGEAcwB0AFYAZQByAHMAaQBvAG4AHhRQAGwAdQBnAGkAbgBHAHUAaQBkAB8WUwB5AHMAdABlAG0ALgBHAHUAaQBkACBIYQBmAGMAZAA1ADQANAA2AC0ANAA5ADEANAAtADQAZgBlADcALQBiAGIANwA4AC0AOQBiAGYAZgBlAGIANwAwAGYAZAAxADcAIRRVAHAAZABhAHQAZQBJAG4AZgBvACJMewBiADAAMwAzADYANgBhADgALQBiADUAYwAwAC0ANABiADkAYQAtAGEAMAAwAGUALQBlAGIAOAA2ADAAMQAxADEAMAA0AGMAMwB9ACMOVQBwAGQAYQB0AGUAcwAkTHsAMQA4ADYAOABmAGYAYwA5AC0AZQA0AGYAYwAtADQANQAzADIALQBhAGMAMAA2AC0AMQBlADMAOQBiAGIANQA1ADcAYgA2ADkAfQAlTHsAYQA1AGIAZAA0ADgAYwAzAC0AMABkADEANwAtADQAMQBiADUALQBiADEANgA0AC0ANQBmAGMANgBhAGQAMgBiADkANgBiADcAfQAmFk8AYgBqAGUAYwB0AHMAVAB5AHAAZQAnVFUAcABkAGEAdABlAEwAYQBuAGcAdQBhAGcAZQBNAG8AZABlAGwARgBvAHIAQwBvAG4AdgBlAHIAdABpAGIAbABlAEwAaQBiAHIAYQByAGkAZQBzACgQTABpAGIAVABpAHQAbABlACkUTABpAGIAQwBvAG0AcABhAG4AeQAqHlUAcABkAGEAdABlAFAAcgBvAHYAaQBkAGUAcgBzACs4UwB5AHMAdABlAG0ALgBDAG8AbABsAGUAYwB0AGkAbwBuAHMALgBIAGEAcwBoAHQAYQBiAGwAZQAsEnYAaQBzAHUAZQBsAGUAbQBzAC1INgBjAGIAMQBjAGQAZQAxAC0AZAA1AGQAYwAtADQAYQAzAGIALQA5ADAANQA0AC0AMgAxAGYAYQA3ADUANgBhADMAZgBhADQALihJAG4AdABlAHIAZgBhAGMAZQBWAGUAcgBzAGkAbwBuAEkAbgBmAG8AL0x7AGMANgAxADEAZQA0ADAAMAAtADcAZgBiADkALQA0AGMAMwA1AC0AYgA5AGEAYwAtADQAZQAzADEANABiADUAOQA5ADYANAAzAH0AMBhNAGEAagBvAHIAVgBlAHIAcwBpAG8AbgAxGE0AaQBuAG8AcgBWAGUAcgBzAGkAbwBuADIMTABlAGcAYQBjAHkAMzBMAGEAbgBnAHUAYQBnAGUATQBvAGQAZQBsAFYAZQByAHMAaQBvAG4ASQBuAGYAbwA0MEwAbwBhAGQATABpAGIAcgBhAHIAaQBlAHMASQBuAHQAbwBQAHIAbwBqAGUAYwB0ADUaQwBvAG0AcABhAHQAaQBiAGkAbABpAHQAeQDQAAIaA9ADAS0E0AUGGgfQBwgaAUUHCQjQAAkaBEUKCwQDAAAABQAAAA0AAAAAAAAA0AwLrQIAAADQDQEtDtAPAS0Q0AAJGgRFCgsEAwAAAAUAAAANAAAAKAAAANAMC60BAAAA0A0BLRHQDwEtENAACRoERQoLBAMAAAAFAAAADQAAAAAAAADQDAutAgAAANANAS0S0A8BLRDQAAkaBEUKCwQDAAAABQAAAA0AAAAUAAAA0AwLrQIAAADQDQEtE9APAS0Q0AAJGgRFCgsEAwAAAAUAAAANAAAAAAAAANAMC60CAAAA0A0BLRTQDwEtENAACRoERQoLBAMAAAAFAAAADQAAAAAAAADQDAutAgAAANANAS0V0A8BLRDQAAkaBEUKCwQDAAAABQAAAA0AAAAAAAAA0AwLrQIAAADQDQEtFtAPAS0X0AAJGgRFCgsEAwAAAAUAAAANAAAAKAAAANAMC60EAAAA0A0BLRjQDwEtENAZGq0BRRscAdAAHBoCRR0LBAMAAAAFAAAADQAAAAAAAADQHh8tINAhIhoCRSMkAtAAJRoFRQoLBAMAAAADAAAAAAAAAAoAAADQJgutAAAAANADAS0n0CgBLRHQKQEtENAAJRoFRQoLBAMAAAADAAAAAAAAAAoAAADQJgutAQAAANADAS0n0CgBLRHQKQEtEJoqKwFFAAEC0AABLSzQAAEtF9AAHy0t0C4vGgPQMAutAQAAANAxC60XAAAA0DIarQDQMy8aA9AwC60CAAAA0DELrQMAAADQMhqtANA0Gq0A0DUarQA=</v>
                  </d>
                </o>
              </d>
              <d n="Values" t="Hashtable" />
            </o>
                </Data>
                <TypeList>
                  <Type n="Hashtable">System.Collections.Hashtable</Type>
                  <Type n="OptionKey">{54dd0eac-a6d8-46f2-8c27-2f43c7e49861}</Type>
                  <Type n="String">System.String</Type>
                </TypeList>
              </XmlArchive>
            </PlcProjectOptions>
          </ProjectExtensions>
        </Project>
        """;
}