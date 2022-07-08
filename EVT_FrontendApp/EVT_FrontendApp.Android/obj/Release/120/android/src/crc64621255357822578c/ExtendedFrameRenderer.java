package crc64621255357822578c;


public class ExtendedFrameRenderer
	extends crc64ee486da937c010f4.FrameRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("EVT_FrontendApp.Droid.Renderers.ExtendedFrameRenderer, eVote", ExtendedFrameRenderer.class, __md_methods);
	}


	public ExtendedFrameRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == ExtendedFrameRenderer.class)
			mono.android.TypeManager.Activate ("EVT_FrontendApp.Droid.Renderers.ExtendedFrameRenderer, eVote", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public ExtendedFrameRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == ExtendedFrameRenderer.class)
			mono.android.TypeManager.Activate ("EVT_FrontendApp.Droid.Renderers.ExtendedFrameRenderer, eVote", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public ExtendedFrameRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == ExtendedFrameRenderer.class)
			mono.android.TypeManager.Activate ("EVT_FrontendApp.Droid.Renderers.ExtendedFrameRenderer, eVote", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
