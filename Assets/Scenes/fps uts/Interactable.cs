using UnityEngine;
using TMPro; // Wajib ditambahkan agar bisa mengubah tulisan Score di layar

public class Interactable : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    
    [Header("Detektif HP")]
    public TextMeshProUGUI debugText; // Kantong khusus untuk memunculkan pesan di layar

    public float pickUpRange = 5f;
    public float moveSpeed = 10f;
    public float rotationSensitivity = 3f;

    private GameObject heldObj;
    private Rigidbody heldObjRb;
    [SerializeField] private PlayerInputHandler inputHandler;

    void Update()
    {
        // Kalau sedang pegang barang, jalankan fungsi memindahkan barang ke depan wajah
        if (heldObj != null)
        {
            MoveObject();
        }
    }

    // FUNGSI KHUSUS TOMBOL MOBILE
    public void MobilePickupOrDrop()
    {
        // 1. Cek apakah tombol benar-benar tersambung dan merespons
        if (debugText != null) debugText.text = "Tombol Merespons!";
        
        if (heldObj == null)
        {
            TryPickUp();
        }
    }

    void TryPickUp()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // 2. Cek benda apa yang sebenarnya tertabrak oleh laser
            if (debugText != null) debugText.text = "Nabrak: " + hit.transform.name;

            if (hit.transform.CompareTag("pickup"))
            {
                PickUpObject(hit.transform.gameObject);
                
                // 3. Cek apakah kodenya berhasil mengeksekusi pengambilan
                if (debugText != null) debugText.text = "Sukses Ambil: " + hit.transform.name;
            }
            else
            {
                // 4. Cek kalau ternyata Tag-nya salah atau belum terpasang
                if (debugText != null) debugText.text = "Gagal! Tag-nya: " + hit.transform.tag;
            }
        }
        else
        {
            // 5. Cek kalau lasernya kependekan atau meleset
            if (debugText != null) debugText.text = "Meleset! Coba maju sedikit.";
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        heldObj = pickUpObj;
        heldObjRb = heldObj.GetComponent<Rigidbody>();

        heldObjRb.useGravity = false;
        heldObjRb.linearDamping = 10;

        Physics.IgnoreCollision(
            heldObj.GetComponent<Collider>(),
            player.GetComponent<Collider>(),
            true
        );
    }

    void MoveObject()
    {
        Vector3 direction = holdPos.position - heldObj.transform.position;
        heldObjRb.linearVelocity = direction * moveSpeed;
    }
}