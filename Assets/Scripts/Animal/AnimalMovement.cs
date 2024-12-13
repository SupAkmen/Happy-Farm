using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AnimalMovement : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] float cooldownTime;
    float cooldownTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cooldownTimer = UnityEngine.Random.Range(0, cooldownTime);
    }

    void Update()
    {
        Wander();
    }

    public void ToggleMovement(bool enabled)
    {
        agent.enabled = enabled;
    }

    private void Wander()
    {
        // Kiểm tra xem agent có hoạt động và đã được đặt trên NavMesh không
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("NavMeshAgent không nằm trên NavMesh.");
            return;
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        // Đảm bảo agent không di chuyển và đã đến đích
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Tạo một hướng ngẫu nhiên
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10;

            // Cộng thêm hướng ngẫu nhiên với vị trí hiện tại của con vật
            randomDirection += transform.position;

            NavMeshHit hit;

            // Lấy vị trí hợp lệ gần nhất trên NavMesh
            if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))
            {
                Vector3 targetPos = hit.position;

                // Đặt điểm đích mới
                agent.SetDestination(targetPos);

                cooldownTimer = cooldownTime;
            }
        }
    }
}
