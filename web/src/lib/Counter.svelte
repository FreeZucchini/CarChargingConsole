<script>
  let charging = $state(false)

  async function toggle() {
    const endpoint = charging ? "charge/stop" : "charge/start"; 
    const API_BASE = "https://carchargingconsole-api.azurewebsites.net/api";
    try {
        const response = await fetch(`${API_BASE}/${endpoint}`, {
          method: "POST"
        });
        if (!response.ok) {
          throw new Error(`Request failed: ${response.status}`);
        }

        charging = !charging;
        console.log(`${endpoint} succeeded`);
      } catch (err) {
        console.log(`${endpoint} failed: `, err);
      }
    }
      
</script>

<button type="button" class="counter" onclick={toggle}>{charging ? "Charging" : "Not Charging"}</button>
