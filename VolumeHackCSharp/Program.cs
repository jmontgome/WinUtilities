using System.Runtime.InteropServices;

namespace MicVolume
{
    class Program
    {
        static void Main()
        {
            // Create device enumerator
            var deviceEnumerator = new MMDeviceEnumerator() as IMMDeviceEnumerator;

            // Get default recording device (microphone)
            IMMDevice device;
            deviceEnumerator.GetDefaultAudioEndpoint(
                EDataFlow.eCapture,
                ERole.eMultimedia,
                out device);

            // Get volume interface
            Guid IID_IAudioEndpointVolume =
                typeof(IAudioEndpointVolume).GUID;

            object obj;
            device.Activate(
                ref IID_IAudioEndpointVolume,
                CLSCTX.ALL,
                IntPtr.Zero,
                out obj);

            IAudioEndpointVolume volume =
                (IAudioEndpointVolume)obj;

            // Set mic volume (0.0 - 1.0)
            volume.SetMasterVolumeLevelScalar(1.0f, Guid.Empty);

            Console.WriteLine("Microphone volume set to 70%");
        }
    }

    enum EDataFlow
    {
        eRender,
        eCapture,
        eAll
    }

    enum ERole
    {
        eConsole,
        eMultimedia,
        eCommunications
    }

    [Flags]
    enum CLSCTX : uint
    {
        INPROC_SERVER = 0x1,
        INPROC_HANDLER = 0x2,
        LOCAL_SERVER = 0x4,
        ALL = INPROC_SERVER | INPROC_HANDLER | LOCAL_SERVER
    }

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    class MMDeviceEnumerator
    {
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
    interface IMMDeviceEnumerator
    {
        int NotImpl1();

        [PreserveSig]
        int GetDefaultAudioEndpoint(
            EDataFlow dataFlow,
            ERole role,
            out IMMDevice ppDevice);
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("D666063F-1587-4E43-81F1-B948E807363F")]
    interface IMMDevice
    {
        [PreserveSig]
        int Activate(
            ref Guid iid,
            CLSCTX dwClsCtx,
            IntPtr pActivationParams,
            [MarshalAs(UnmanagedType.Interface)] out object ppInterface);

        int OpenPropertyStore(int stgmAccess, out object ppProperties);

        int GetId([MarshalAs(UnmanagedType.LPWStr)] out string ppstrId);

        int GetState(out int pdwState);
    }

    [ComImport]
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IAudioEndpointVolume
    {
        int RegisterControlChangeNotify(IntPtr pNotify);
        int UnregisterControlChangeNotify(IntPtr pNotify);
        int GetChannelCount(out uint channelCount);

        int SetMasterVolumeLevel(float levelDB, Guid eventContext);

        int SetMasterVolumeLevelScalar(
            float level,
            Guid eventContext);

        int GetMasterVolumeLevel(out float levelDB);

        int GetMasterVolumeLevelScalar(out float level);

        int SetChannelVolumeLevel(
            uint channelNumber,
            float levelDB,
            Guid eventContext);

        int SetChannelVolumeLevelScalar(
            uint channelNumber,
            float level,
            Guid eventContext);

        int GetChannelVolumeLevel(
            uint channelNumber,
            out float levelDB);

        int GetChannelVolumeLevelScalar(
            uint channelNumber,
            out float level);

        int SetMute(
            [MarshalAs(UnmanagedType.Bool)] bool isMuted,
            Guid eventContext);

        int GetMute(out bool isMuted);

        int GetVolumeStepInfo(
            out uint step,
            out uint stepCount);

        int VolumeStepUp(Guid eventContext);

        int VolumeStepDown(Guid eventContext);

        int QueryHardwareSupport(out uint hardwareSupportMask);

        int GetVolumeRange(
            out float volumeMindB,
            out float volumeMaxdB,
            out float volumeIncrementdB);
    }
}