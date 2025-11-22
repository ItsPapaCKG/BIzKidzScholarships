import type { taskConfigData } from "../../models/tastConfigData";

function Contest({ taskData }: { taskData: taskConfigData }) {

    return (
        <>
            <h1>{ taskData.taskPromptTitle }</h1>

            <p>{taskData.taskPromptSubtitle}</p>

            <ul>
                <li>December 4, 2025 @ 12:00pm / The Docks at Port Charlotte</li>
                <li>December 15, 2002 @ 12:00am / Imaginarium in Ft Myers</li>
            </ul>
        </>
    );
}

export default Contest;