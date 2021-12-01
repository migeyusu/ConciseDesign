using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ConciseDesign.WPF.Extension
{
    /// <summary>
    /// win8下无法使用
    /// </summary>
    public static class AREOExtend
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Margins
        {
            public int left;
            public int right;
            public int top;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DwmBlurbehind
        {
            public uint dwFlags;
            [MarshalAs(UnmanagedType.Bool)] public bool fEnable;
            public IntPtr hRgnBlur;
            [MarshalAs(UnmanagedType.Bool)] public bool fTransitionOnMaximized;
            public const uint DwmBbEnable = 0x01;
            public const uint DwmBbBlurregion = 0x02;
            public const uint DwmBbTransitiononmaximized = 0x04;
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int en);

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins margin);

        [DllImport("dwmapi.dll")] //, CharSet = CharSet.Auto, PreserveSig = false, CallingConvention = CallingConvention.Cdecl
        public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, ref DwmBlurbehind pBlurBehind);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableComposition(bool bEnable);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmGetColorizationColor(out int pcrColorization,
            [MarshalAs(UnmanagedType.Bool)] out bool pfOpaqueBlend);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("user32.dll")]
        public static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [StructLayout(LayoutKind.Sequential)]
        public struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        public enum WindowCompositionAttribute
        {
            WcaUndefined = 0,
            WcaNcrenderingEnabled = 1,
            WcaNcrenderingPolicy = 2,
            WcaTransitionsForcedisabled = 3,
            WcaAllowNcpaint = 4,
            WcaCaptionButtonBounds = 5,
            WcaNonclientRtlLayout = 6,
            WcaForceIconicRepresentation = 7,
            WcaExtendedFrameBounds = 8,
            WcaHasIconicBitmap = 9,
            WcaThemeAttributes = 10,
            WcaNcrenderingExiled = 11,
            WcaNcadornmentinfo = 12,
            WcaExcludedFromLivepreview = 13,
            WcaVideoOverlayActive = 14,
            WcaForceActivewindowAppearance = 15,
            WcaDisallowPeek = 16,
            WcaCloak = 17,
            WcaCloaked = 18,
            WcaAccentPolicy = 19,
            WcaFreezeRepresentation = 20,
            WcaEverUncloaked = 21,
            WcaVisualOwner = 22,
            WcaLast = 23
        }

        public enum AccentState
        {
            AccentDisabled = 0,
            AccentEnableGradient = 1,
            AccentEnableTransparentgradient = 2,
            AccentEnableBlurbehind = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_INVALID_STATE = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        /// <summary>
        /// Drops a standard shadow to a WPF Window, even if the window is border less. Only works with DWM (Windows Vista or newer).
        /// This method is much more efficient than setting AllowsTransparency to true and using the DropShadow effect,
        /// as AllowsTransparency involves a huge performance issue (hardware acceleration is turned off for all the window).
        /// </summary>
        /// <param name="window">Window to which the shadow will be applied</param>
        public static void DropShadowToWindow(this Window window)
        {
            if (!DropShadow(window))
            {
                window.SourceInitialized += new EventHandler(window_SourceInitialized);
            }
        }

        private static void window_SourceInitialized(object sender, EventArgs e)
        {
            Window window = (Window) sender;

            DropShadow(window);

            window.SourceInitialized -= new EventHandler(window_SourceInitialized);
        }

        /// <summary>
        /// The actual method that makes API calls to drop the shadow to the window
        /// </summary>
        /// <param name="window">Window to which the shadow will be applied</param>
        /// <returns>True if the method succeeded, false if not</returns>
        private static bool DropShadow(Window window)
        {
            try
            {
                WindowInteropHelper helper = new WindowInteropHelper(window);
                int val = 2;
                int ret1 = DwmSetWindowAttribute(helper.Handle, 2, ref val, 4);

                if (ret1 == 0)
                {
                    Margins m = new Margins {bottom = 0, left = 0, right = 0, top = 10};
                    int ret2 = DwmExtendFrameIntoClientArea(helper.Handle, ref m);
                    return ret2 == 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                // Probably dwmapi.dll not found (incompatible OS)
                return false;
            }
        }

        private static void Win10AreoEffectOn(IntPtr hwnd)
        {
            var accentPolicy = new AccentPolicy()
            {
                AccentState = AccentState.AccentEnableBlurbehind,
                AccentFlags = 0,
                AnimationId = 0,
                GradientColor = 0
            };
            // Dispatcher
            var versionValue = SystemInfo.Version.Value;
            if (versionValue >= VersionInfos.Windows10_1903)
            {
                //Windows10 1903以后，如果使用accent_enable_acrylicblurbehind的话，窗口的拖拽移动等就不再跟随鼠标操作了。
                accentPolicy.AccentState = AccentState.AccentEnableBlurbehind;
            }
            else if (versionValue >= VersionInfos.Windows10_1809)
            {
                accentPolicy.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
            }
            else if (versionValue >= VersionInfos.Windows10)
            {
                accentPolicy.AccentState = AccentState.AccentEnableBlurbehind;
            }
            else
            {
                accentPolicy.AccentState = AccentState.AccentEnableTransparentgradient;
            }

            var accentPolicySize = Marshal.SizeOf(accentPolicy);
            var allocHGlobal = Marshal.AllocHGlobal(accentPolicySize);
            Marshal.StructureToPtr(accentPolicy, allocHGlobal, false);
            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WcaAccentPolicy,
                SizeOfData = accentPolicySize,
                Data = allocHGlobal
            };
            SetWindowCompositionAttribute(hwnd, ref data);
            Marshal.FreeHGlobal(allocHGlobal);
        }

        private static void Win10AreoEffectOff(IntPtr hwnd)
        {
            var ap = new AccentPolicy()
            {
                AccentState = AccentState.AccentDisabled,
                AccentFlags = 0,
                AnimationId = 0,
                GradientColor = 0
            };
            var size = Marshal.SizeOf(ap);
            var apptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(ap, apptr, false);
            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WcaAccentPolicy,
                SizeOfData = size,
                Data = apptr
            };
            SetWindowCompositionAttribute(hwnd, ref data);
            Marshal.FreeHGlobal(apptr);
        }

        private static void Win7AreoEffectOn(IntPtr hwnd)
        {
            var db = new DwmBlurbehind
            {
                fEnable = true,
                hRgnBlur = IntPtr.Zero,
                fTransitionOnMaximized = false,
                dwFlags = 1,
            };
            DwmEnableBlurBehindWindow(hwnd, ref db);
        }

        private static void Win7AreoEffectOff(IntPtr hwnd)
        {
            var db = new DwmBlurbehind
            {
                fEnable = false,
                hRgnBlur = IntPtr.Zero,
                fTransitionOnMaximized = false,
                dwFlags = 1,
            };
            DwmEnableBlurBehindWindow(hwnd, ref db);
        }

        private static void InternalAreoEffectOn(IntPtr hwnd)
        {
            var os = Environment.OSVersion.Version;
            if (os.Major == 6)
            {
                if (os.Minor <= 1)
                {
                    Win7AreoEffectOn(hwnd);
                }
                else
                {
                    Win10AreoEffectOn(hwnd);
                }
            }
            else if (os.Major == 10)
            {
                Win10AreoEffectOn(hwnd);
            }
        }

        public static void AreoEffectOn(this Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            InternalAreoEffectOn(hwnd);
        }

        private static void InternalAreoEffectOff(IntPtr hwnd)
        {
            var os = Environment.OSVersion.Version;
            if (os.Major == 6)
            {
                switch (os.Minor)
                {
                    case 1:
                        Win7AreoEffectOff(hwnd);
                        return;
                    case 2:
                        Win10AreoEffectOff(hwnd);
                        return;
                }
            }
            else if (os.Major == 10 && os.Minor == 0)
            {
                Win10AreoEffectOff(hwnd);
            }
        }


        public static void AreoEffectOff(this Window window)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            InternalAreoEffectOn(hwnd);
        }
    }

    internal sealed class VersionInfos
    {
        public static VersionInfo Windows7
        {
            get { return new VersionInfo(6, 1, 7600); }
        }

        public static VersionInfo Windows7_SP1
        {
            get { return new VersionInfo(6, 1, 7601); }
        }

        public static VersionInfo Windows8
        {
            get { return new VersionInfo(6, 2, 9200); }
        }

        public static VersionInfo Windows8_1
        {
            get { return new VersionInfo(6, 3, 9600); }
        }

        public static VersionInfo Windows10
        {
            get { return new VersionInfo(10, 0, 10240); }
        }

        public static VersionInfo Windows10_1511
        {
            get { return new VersionInfo(10, 0, 10586); }
        }

        public static VersionInfo Windows10_1607
        {
            get { return new VersionInfo(10, 0, 14393); }
        }

        public static VersionInfo Windows10_1703
        {
            get { return new VersionInfo(10, 0, 15063); }
        }

        public static VersionInfo Windows10_1709
        {
            get { return new VersionInfo(10, 0, 16299); }
        }

        public static VersionInfo Windows10_1803
        {
            get { return new VersionInfo(10, 0, 17134); }
        }

        public static VersionInfo Windows10_1809
        {
            get { return new VersionInfo(10, 0, 17763); }
        }

        public static VersionInfo Windows10_1903
        {
            get { return new VersionInfo(10, 0, 18362); }
        }
    }

    internal struct VersionInfo : IEquatable<VersionInfo>, IComparable<VersionInfo>, IComparable
    {
        public int Major;
        public int Minor;
        public int Build;

        public VersionInfo(int major, int minor, int build)
        {
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
        }

        public bool Equals(VersionInfo other)
        {
            return this.Major == other.Major && this.Minor == other.Minor && this.Build == other.Build;
        }

        public override bool Equals(object obj)
        {
            return (obj is VersionInfo other) && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Major.GetHashCode() ^ this.Minor.GetHashCode() ^ this.Build.GetHashCode();
        }

        public static bool operator ==(VersionInfo left, VersionInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VersionInfo left, VersionInfo right)
        {
            return !(left == right);
        }


        public int CompareTo(VersionInfo other)
        {
            if (this.Major != other.Major)
            {
                return this.Major.CompareTo(other.Major);
            }
            else if (this.Minor != other.Minor)
            {
                return this.Minor.CompareTo(other.Minor);
            }
            else if (this.Build != other.Build)
            {
                return this.Build.CompareTo(other.Build);
            }
            else
            {
                return 0;
            }
        }

        public int CompareTo(object obj)
        {
            if (!(obj is VersionInfo other))
            {
                throw new ArgumentException();
            }

            return this.CompareTo(other);
        }

        public static bool operator <(VersionInfo left, VersionInfo right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(VersionInfo left, VersionInfo right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(VersionInfo left, VersionInfo right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(VersionInfo left, VersionInfo right)
        {
            return left.CompareTo(right) >= 0;
        }

        public override string ToString()
        {
            return $"{this.Major}.{this.Minor}.{this.Build}";
        }
    }

    internal class SystemInfo
    {
        public static Lazy<VersionInfo> Version { get; private set; } = new Lazy<VersionInfo>(() => GetVersionInfo());

        internal static VersionInfo GetVersionInfo()
        {
            var regkey =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\",
                    false);
            if (regkey == null) return default(VersionInfo);
            var majorValue = regkey.GetValue("CurrentMajorVersionNumber");
            var minorValue = regkey.GetValue("CurrentMinorVersionNumber");
            var buildValue = (string) regkey.GetValue("CurrentBuild", 7600);
            var canReadBuild = int.TryParse(buildValue, out var build);
            var defaultVersion = System.Environment.OSVersion.Version;

            if (majorValue is int major && minorValue is int minor && canReadBuild)
            {
                return new VersionInfo(major, minor, build);
            }
            else
            {
                return new VersionInfo(defaultVersion.Major, defaultVersion.Minor, defaultVersion.Revision);
            }
        }
    }
}
