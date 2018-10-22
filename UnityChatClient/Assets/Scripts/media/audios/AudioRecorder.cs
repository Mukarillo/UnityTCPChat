using UnityEngine;
using UnityEngine.Events;

public class AudioRecorder
{
	private AudioClip mRecordingClip;
	private int mSampleRate = 44100;
	private float mTrimCutoff = .01f;
	private UnityAction<AudioClip> mOnCompleteCallback;
	private bool mIsRecording;
       
	public void StartRecording(UnityAction<AudioClip> completeCallback, int maxClipLenth = 99)
	{
		mOnCompleteCallback = completeCallback;
		mRecordingClip = Microphone.Start(null, true, maxClipLenth, mSampleRate);

		mIsRecording = true;

		System.Threading.Timer timer = null;
		timer = new System.Threading.Timer((obj) =>
        {
			if (mIsRecording)
			{
				MainThread.invoke(EndRecording);
				timer.Dispose();
			}
		}, null, 1000 * maxClipLenth, System.Threading.Timeout.Infinite);
	}

    public void EndRecording()
	{
		mIsRecording = false;
		Microphone.End("Built-in Microphone");

		AudioClip trimmedClip = SavWav.TrimSilence(mRecordingClip, mTrimCutoff);
		mOnCompleteCallback.Invoke(trimmedClip);      
	}
}
