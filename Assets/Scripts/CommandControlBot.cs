using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BotState
{
    STANDART,
    QUEUE
}

public class CommandControlBot : MonoBehaviour
{
    private NavMeshAgent agent;

    private Queue<Command> commands = new Queue<Command>();
    private Command currentCommand;

    public BotState botState;

    private void Awake() => botState = BotState.STANDART;
    private void Start() => agent = GetComponent<NavMeshAgent>();

    private void Update()
    {
        ListenForCommands();
        ProcessCommands();
    }

    private void ListenForCommands()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (botState == BotState.QUEUE)
            {
                while(commands.Count > 0)
                {
                    commands.Dequeue();
                }
            }
             agent.SetDestination(transform.position);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out var hitInfo))
            {
                if(botState == BotState.STANDART)
                {
                    agent.SetDestination(hitInfo.point);
                }
                else if(botState == BotState.QUEUE)
                {
                    var moveCommand = new MoveCommand(hitInfo.point, agent);
                    commands.Enqueue(moveCommand);
                }
            }
        }
        
    }
    
    private void ProcessCommands()
    {
        if (currentCommand != null && currentCommand.IsFinished == false)
            return;

        if (commands.Count == 0)
            return;

        currentCommand = commands.Dequeue();
        currentCommand.Execute();
    }

    internal class MoveCommand: Command
    {
        private readonly Vector3 _destination;
        private readonly NavMeshAgent agent;

        public MoveCommand(Vector3 destination, NavMeshAgent agent)
        {
            _destination = destination;
            this.agent = agent;
        }

        public override void Execute()
        {
            agent.SetDestination(_destination);
        }

        public override bool IsFinished => agent.remainingDistance < 0.1f;
    }
}
