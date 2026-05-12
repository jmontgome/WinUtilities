#define COBJMACROS

#include <windows.h>
#include <initguid.h>
DEFINE_GUID(IID_IMMDeviceEnumerator,
    0xA95664D2, 0x9614, 0x4F35, 0xA7, 0x46, 0xDE, 0x8D, 0xB6, 0x36, 0x17, 0xE6);
DEFINE_GUID(CLSID_MMDeviceEnumerator,
    0xBCDE0395, 0xE52F, 0x467C, 0x8E, 0x3D, 0xC4, 0x57, 0x92, 0x91, 0x69, 0x2E);
DEFINE_GUID(IID_IAudioEndpointVolume,
    0x5CDF2C82, 0x841E, 0x4546, 0x97, 0x22, 0x0C, 0xF7, 0x40, 0x78, 0x22, 0x9A);
#include <mmdeviceapi.h>
#include <endpointvolume.h>
#include <stdlib.h>
#include <stdio.h>

int main(int argc, char* argv[])
{
    HRESULT hr;

    float volumeLevel = 1.0f;

    if (argc > 1) {
        char* end;
        float testFloat = strtof(argv[1], &end);
        
        if (*end != '\0' && *end != '%') {
            printf("Invalid number\n");
            return 1;
        }

        if (testFloat > 100.0f ||
            testFloat < 0.0f) {
            printf("Please enter a number between 0-100.");
            return 1;
        }

        volumeLevel = testFloat / 100;

        if (volumeLevel < 0.0f) volumeLevel = 0.0f;
        if (volumeLevel > 1.0f) volumeLevel = 1.0f;
    }

    IMMDeviceEnumerator* enumerator = NULL;
    IMMDevice* device = NULL;
    IAudioEndpointVolume* endpointVolume = NULL;

    hr = CoInitialize(NULL);
    if (FAILED(hr)) {
        CoUninitialize();
        return -1;
    }

    hr = CoCreateInstance(
        &CLSID_MMDeviceEnumerator,
        NULL,
        CLSCTX_ALL,
        &IID_IMMDeviceEnumerator,
        (void**)&enumerator
    );

    if (FAILED(hr)) {
        printf("CoCreateInstance failed\n");
        CoUninitialize();
        return -1;
    }

    hr = IMMDeviceEnumerator_GetDefaultAudioEndpoint(
        enumerator,
        eCapture,
        eMultimedia,
        &device
    );

    if (FAILED(hr)) {
        printf("GetDefaultAudioEndpoint failed\n");
        CoUninitialize();
        return -1;
    }

    hr = IMMDevice_Activate(
        device,
        &IID_IAudioEndpointVolume,
        CLSCTX_ALL,
        NULL,
        (void**)&endpointVolume
    );

    if (FAILED(hr)) {
        printf("Activate failed\n");
        CoUninitialize();
        return -1;
    }

    IAudioEndpointVolume_SetMasterVolumeLevelScalar(
        endpointVolume,
        volumeLevel,
        NULL
    );

    printf("Mic volume set to %.0f%%\n", volumeLevel * 100);

    IAudioEndpointVolume_Release(endpointVolume);
    IMMDevice_Release(device);
    IMMDeviceEnumerator_Release(enumerator);

    CoUninitialize();

    return 0;
}