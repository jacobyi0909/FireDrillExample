using UnityEngine;

public class NPCAnimEvent : MonoBehaviour
{
    Animator anim;
    NPC npc;
    void Start()
    {
        anim = GetComponent<Animator>();
        npc = GetComponentInParent<NPC>();
    }

    void Update()
    {
        
    }

    public void OnGetupFinished()
    {
        npc.SetState(NPC.State.Idle);
        anim.SetTrigger("Idle");
    }
}
