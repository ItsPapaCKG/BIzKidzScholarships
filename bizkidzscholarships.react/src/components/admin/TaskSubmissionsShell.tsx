import { useEffect, useState } from "react";
import { UseAdminContext } from "../../contexts/AdminContext";
import TaskSubmissions from "./TaskSubmissions";

function TaskSubmissionsShell() {
    const context = UseAdminContext();

    const [selectedTask, setSelectedTask] = useState<number | undefined>();
    const [tasks] = [context.tasks];
    const [GetTasks] = [context.getTasks];

    useEffect(() => {
        GetTasks();
    }, [])

  return (
      <>
          {tasks && (
              <div className="container p-3">
                  <div className="row">
                      <div className="col">
                          <div className="row">
                              <h1>Submissions</h1>
                          </div>
                          <div className="row">
                              <select className="form-select" onChange={(e) => setSelectedTask(Number(e.target.value))} value={selectedTask ?? ""}>
                                  <option value="" disabled>Please select a task..</option>
                                  {!tasks?.error && tasks?.results.map((t) => {
                                      return <option value={t.id} key={ t.id }>{t.taskTitle}</option>
                                  })}
                              </select>
                          </div>
                      </div>

                  </div>
                  <div className="row mt-5">
                      {selectedTask &&
                          <>
                              <TaskSubmissions taskId={selectedTask} />
                          </>
                      }
                  </div>
              </div>
          
          )}
      </>
  );
}

export default TaskSubmissionsShell;