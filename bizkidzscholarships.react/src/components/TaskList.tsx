import { useTasksContext } from "../context";
import Task from "./Task";

function TasksList() {
    const tasks = useTasksContext();

  return (
      <div>
          {
              tasks.map((task, index, array) => {
                  return <Task task={ task } />
              }) 
          }
      </div>
  );
}

export default TasksList;