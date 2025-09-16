import { useUserPointsContext } from "../context";


function UserPoints() {
    const points = useUserPointsContext();

  return (
      <div>
          <div>
            <p>Points:</p>
              <p>{points.Total}</p>
          </div>

          <div>
              <p>Entries:</p>
              <p>{points.Entries}</p>
          </div>
      </div>
  );
}

export default UserPoints;