package crc640e9d4af07a6bfefb;


public class OnUserFilterSnapshotListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.firebase.firestore.EventListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onEvent:(Ljava/lang/Object;Lcom/google/firebase/firestore/FirebaseFirestoreException;)V:GetOnEvent_Ljava_lang_Object_Lcom_google_firebase_firestore_FirebaseFirestoreException_Handler:Firebase.Firestore.IEventListenerInvoker, Xamarin.Firebase.Firestore\n" +
			"";
		mono.android.Runtime.register ("EVT_FrontendApp.Droid.Services.ServiceListeners.OnUserFilterSnapshotListener, eVote", OnUserFilterSnapshotListener.class, __md_methods);
	}


	public OnUserFilterSnapshotListener ()
	{
		super ();
		if (getClass () == OnUserFilterSnapshotListener.class)
			mono.android.TypeManager.Activate ("EVT_FrontendApp.Droid.Services.ServiceListeners.OnUserFilterSnapshotListener, eVote", "", this, new java.lang.Object[] {  });
	}


	public void onEvent (java.lang.Object p0, com.google.firebase.firestore.FirebaseFirestoreException p1)
	{
		n_onEvent (p0, p1);
	}

	private native void n_onEvent (java.lang.Object p0, com.google.firebase.firestore.FirebaseFirestoreException p1);

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
