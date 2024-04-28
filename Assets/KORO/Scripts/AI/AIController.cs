using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _targetFirstPosition;
    
    [SerializeField] private Animator _playerAnimator;
    
    public LayerMask obstacleMask;
    public int obstacleMaskValue;

    // Start is called before the first frame update
    void Start()
    {
        _agent.destination = _targetTransform.position;
        float randomTime = Random.Range(0f, 1f);
        _playerAnimator.Play("Walk",0,randomTime);
        //obstacleMaskValue = LayerMask.NameToLayer("Player");
        InvokeRepeating("CheckForObstacles", 0f, 0.3f);
        //_targetFirstPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (1 > Vector3.Distance(transform.position, _targetTransform.position))
        {
            transform.position = _targetFirstPosition.position;
        }
    }
    void CheckForObstacles()
    {
            RaycastHit hit;
            if (Physics.Raycast(transform.position+ Vector3.up, transform.right, out hit, 2f, obstacleMask))
            {
                hit.collider.gameObject.GetComponent<Player>().GainMoney(4f);
                Debug.DrawRay(transform.position + Vector3.up, transform.right * 2f, Color.blue);

            }
        
        
            if (Physics.Raycast(transform.position+ Vector3.up, -transform.right, out hit, 2f, obstacleMask))
            {
                hit.collider.gameObject.GetComponent<Player>().GainMoney(4f);
                Debug.DrawRay(transform.position + Vector3.up, -transform.right * 2f, Color.red);

            }
            
        
    }
}
