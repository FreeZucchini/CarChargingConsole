<script>
  let setSchedule = $state(false);
  let startchargeTime = $state("");

  async function toggle() {
    const endpoint = setSchedule ? "schedule/cancel" : "schedule/set"; 
    const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";

    const payload = {
      time: startchargeTime
    };

    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: "POST",
        body: JSON.stringify(payload)
      });
        if (!response.ok) {
          throw new Error(`Request failed: ${response.status}`);
        }

        setSchedule = !setSchedule;
        console.log(`${endpoint} succeeded`);
        localStorage.setItem("setSchedule", setSchedule.toString());
    } catch (err) {
        console.log(`${endpoint} failed: `, err);
    }
  }

  if (localStorage.getItem("setSchedule")) {
    const storedScheduleBool = localStorage.getItem("setSchedule");
    setSchedule = storedScheduleBool === "true";
  }
      
</script>

<button type="button" class="counter" onclick={toggle}>{setSchedule ? "Set Schedule" : "Cancel Schedule"}</button>
<input type="time" bind:value={startchargeTime} required/>

