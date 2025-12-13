import { useEffect, useState } from "react";
import { UseTaskContext } from "../../contexts/TaskViewContext";


function UserPoints() {
    const context = UseTaskContext();

    const [points, setPoints] = [context.points, context.setPoints];
    const [RequestPoints] = [context.RequestPoints]
    const [DoPointsRefresh, setPointsRefresh] = [context.DoPointsRefresh, context.setPointsRefresh];
    const [IsError, setIsError] = useState<boolean>(false);

    const getPoints = async() => {
        let p = await RequestPoints();
        
        if (p.IsError) {
          setIsError(true);
        }

        setPoints(p);
      }

    useEffect(() => {
      getPoints();
    }, [])

    useEffect(() => {
      if (DoPointsRefresh) {
        getPoints();
        setPointsRefresh(false);
      }
    }, [DoPointsRefresh])

  if (!points.Loaded) return (<></>)
  else return (
      <div className="card p-3 userpoints border-info flex-fill">
        <div className="card-body">
          <div className="row text-center">
            <div className="col">
            <h4>Points:</h4>
              <p>{points.TotalPoints}</p>
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