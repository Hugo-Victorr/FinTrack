import { Refine } from "@refinedev/core";
import { RefineKbar, RefineKbarProvider } from "@refinedev/kbar";

import { useNotificationProvider } from "@refinedev/antd";
import "@refinedev/antd/dist/reset.css";

import { useKeycloak } from "@react-keycloak/web";
import routerProvider, {
  DocumentTitleHandler,
  UnsavedChangesNotifier,
} from "@refinedev/react-router";
import { App as AntdApp } from "antd";
import { BrowserRouter } from "react-router";
import { FintrackLogo } from "./components/icons/fintrackLogo";
import { ColorModeContextProvider } from "./contexts/color-mode";
import { accessControlProvider } from "./providers/access-control-provider";
import { authProvider } from "./providers/auth-provider";
import { dataProvider } from "./providers/rest-data-provider";
import { appResources, AppRoutes } from "./routes/resources";
import { useEffect, useState } from "react";
import Lottie from "react-lottie";
import splashIcon from "./components/lotties/coin-splash.json";
import "./styles.css";

function App() {
  const { initialized } = useKeycloak();

  const [ready, setReady] = useState(false);

  const mountedStyle = { animation: "inAnimation 750ms ease-in" };
  const unmountedStyle = {
    animation: "outAnimation 750ms ease-out",
    animationFillMode: "forwards",
  };

  useEffect(() => {
    // Wait for Keycloak + a minimum 2s splash delay
    if (initialized) {
      const timer = setTimeout(() => {
        setReady(true);
      }, 2000);
      return () => clearTimeout(timer);
    }
  }, [initialized]);

  if (!ready) {
    return (
      <div className="splash">
        <Lottie
          options={{
            loop: true,
            autoplay: true,
            animationData: splashIcon,
            rendererSettings: {
              preserveAspectRatio: "xMidYMid slice",
            },
          }}
          height={150}
          width={150}
        />
      </div>
    );
  }

  return (
    <BrowserRouter>
      <RefineKbarProvider>
        <ColorModeContextProvider>
          <AntdApp>
            <div
              className="transitionDiv"
              style={initialized ? mountedStyle : unmountedStyle}
            >
              <Refine
                dataProvider={dataProvider("http://localhost:5166/api")}
                notificationProvider={useNotificationProvider}
                routerProvider={routerProvider}
                authProvider={authProvider}
                accessControlProvider={accessControlProvider}
                resources={appResources}
                options={{
                  syncWithLocation: true,
                  warnWhenUnsavedChanges: true,
                  projectId: "G9f8XF-O7GGK6-BspEWd",
                  disableTelemetry: true,
                  title: {
                    icon: <FintrackLogo />,
                    text: "FinTrack Project",
                  },
                }}
              >
                <AppRoutes />
                <RefineKbar />
                <UnsavedChangesNotifier />
                <DocumentTitleHandler />
              </Refine>
            </div>
          </AntdApp>
        </ColorModeContextProvider>
      </RefineKbarProvider>
    </BrowserRouter>
  );
}

export default App;
