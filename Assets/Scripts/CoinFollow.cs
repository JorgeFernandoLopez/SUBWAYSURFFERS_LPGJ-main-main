using UnityEngine;
 
public class CoinFollow : MonoBehaviour
{
    private Transform player;
    [SerializeField]
    private float followSpeed = 10f; // Aumentado para que se sienta más como un imán potente
    [SerializeField]
    private float minimumDistance = 0.3f; // Distancia un poco más amplia para evitar que "orbiten" el centro
    
    private bool canFollow = true;
    private Vector3 originalPosition = Vector3.zero;
 
    private void Awake()
    {
        originalPosition = transform.localPosition;
    }
 
    private void OnEnable()
    {
        canFollow = true;
        player = null;
        if (originalPosition != Vector3.zero) 
            transform.localPosition = originalPosition;
    }
 
    public void StartFollowing(Transform playerTransform)
    {
        // Si ya está siguiendo a alguien, no reiniciamos (evita tirones)
        if (!canFollow) return;
        
        canFollow = false;
        player = playerTransform;
    }
 
    public void Update()
    {
        if (player != null)
        {
            // Movimiento fluido hacia el jugador
            Vector3 targetPosition = player.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
 
            // COMPROBACIÓN DE DISTANCIA (Tu cambio está aquí)
            if (Vector3.Distance(transform.position, targetPosition) < minimumDistance)
            {
                // Intentamos avisar al PlayerCollider para que sume puntos y apague la moneda
                PlayerCollider pc = player.GetComponent<PlayerCollider>();
                
                if (pc != null)
                {
                    pc.CollectCoin(gameObject);
                }
                else
                {
                    // Respaldo de seguridad: si no encuentra el script, la moneda se apaga sola
                    gameObject.SetActive(false);
                }
 
                // Limpiamos la referencia para que no intente seguir nada mientras está apagada
                player = null;
            }
        } 
    }
}
 