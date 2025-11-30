import { UseTaskContext } from "../../contexts/TaskViewContext";


function UserPoints() {
    const context = UseTaskContext();

    const [points] = [context.points];

  return (
      <div className="card p-3 userpoints h-100 border-info">
        <div className="card-body d-flex flex-column justify-content-center">
          <div className="row text-center">
            <div className="col">
            <h4>Points:</h4>
              <p>{points.Total}</p>
              </div>
          </div>

          <div className="row text-center">
            <div className="col">
              <h4>Entries:</h4>
              <p>{points.Entries}</p>
              </div>
          </div>
          </div>
      </div>
  );
}

export default UserPoints;