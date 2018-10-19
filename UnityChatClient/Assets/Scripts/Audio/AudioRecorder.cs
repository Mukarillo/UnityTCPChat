using UnityEngine;
using UnityEngine.Events;

public class AudioRecorder
{
	private AudioClip mRecordingClip;
	private int mSampleRate = 44100;
	private float mTrimCutoff = .01f;
	private UnityAction<AudioClip> mOnCompleteCallback;
       
	public void StartRecording(UnityAction<AudioClip> completeCallback, int maxClipLenth = 99)
	{
		mOnCompleteCallback = completeCallback;
		mRecordingClip = Microphone.Start("Built-in Microphone", true, maxClipLenth, mSampleRate);

		System.Threading.Timer timer = null;
		timer = new System.Threading.Timer((obj) =>
        {
			EndRecording();
            timer.Dispose();
		}, null, 1000 * maxClipLenth, System.Threading.Timeout.Infinite);
	}

    public void EndRecording()
	{
		Microphone.End("Built-in Microphone");

		AudioClip trimmedClip = SavWav.TrimSilence(mRecordingClip, mTrimCutoff);
		mOnCompleteCallback.Invoke(trimmedClip);      
	}
}
