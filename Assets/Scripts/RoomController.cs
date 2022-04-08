using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using agora_gaming_rtc;
using agora_utilities;

public class RoomController : MonoBehaviour
{
    private GameObject manPerfab;
    private const string MemberTag = "Member";
    private const string VideoTag = "Video";
    // Start is called before the first frame update
    void Start()
    {
        manPerfab = (GameObject)Resources.Load("Prefabs/Man");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddUser(uint uid,bool isMySelf) {
        GameObject member = Instantiate(manPerfab);
        member.name = MemberTag + uid.ToString();
        member.transform.SetParent(transform.Find("Members"));
        if (isMySelf) {
            member.GetComponent<AvatarController>().setMySelf(true);
            member.transform.Find("Camera").gameObject.SetActive(true);
        }
        member.transform.Find("IdText").GetComponent<TextMesh>().text = uid.ToString();
        member.transform.Rotate(Vector3.up,90);
        member.transform.localPosition = Vector3.zero;
    }

    public void RemoveUser(uint uid) {
        RemoveObject(MemberTag + uid.ToString());
    }

    public void AddVideo(uint uid) {
        GameObject video = GameObject.CreatePrimitive(PrimitiveType.Quad);
        video.name = VideoTag + uid.ToString();
        VideoSurface videoSurface = video.AddComponent<VideoSurface>();
        videoSurface.SetForUser(uid);
        video.transform.SetParent(transform.Find("Videos"));
        video.transform.localPosition = Vector3.zero;
        video.transform.localScale = new Vector3(3.2f,1.8f,1);
        video.transform.Rotate(Vector3.up,90);
        video.transform.Rotate(Vector3.forward,180);
    }

    public void RemoveVideo(uint uid) {
        RemoveObject(VideoTag + uid.ToString());
    }

    private void RemoveObject(string tag) {
        Debug.Log("RemoveObject: "+tag);
        GameObject go = GameObject.Find(tag);
        if (!ReferenceEquals(go, null))
        {
            Destroy(go);
        }
    }

    public void HandlerMessage(uint uid,byte[] data) {
        GameObject member = GameObject.Find(MemberTag+uid.ToString());
        if (!ReferenceEquals(member, null)) {
            AvatarController controller = member.GetComponent<AvatarController>();
            controller.HandleMessage(data);
        }
    }
}
