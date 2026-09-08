<script>
  let charging = $state(false)

  async function toggle() {
    const endpoint = charging ? "charge/stop" : "charge/start"; 
    const API_BASE = "https://cardashboardbackend-arg8cmfgf6befkhd.westus3-01.azurewebsites.net/api";
    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
        method: "POST"
      });
        if (!response.ok) {
          throw new Error(`Request failed: ${response.status}`);
        }

        charging = !charging;
        console.log(`${endpoint} succeeded`);
        sessionStorage.setItem("charging", charging.toString());
    } catch (err) {
        console.log(`${endpoint} failed: `, err);
    }
  }

  if (sessionStorage.getItem("charging")) {
    const storedCharging = sessionStorage.getItem("charging");
    charging = storedCharging === "true";
  }
      
</script>

<button type="button" class="counter" onclick={toggle}>{charging ? "Charging" : "Not Charging"}</button>
