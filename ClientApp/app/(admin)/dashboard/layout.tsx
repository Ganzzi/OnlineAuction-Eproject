"use client";
import "../../globals.css";
import "../../data-tables-css.css";
import "../../satoshi.css";
import { useState, useEffect } from "react";
import Loader from "@/components/common/Loader";

import Sidebar from "@/components/Dashboard/Sidebar";
import Header from "@/components/Dashboard/Header";
import { useGlobalState } from "@/context/globalState";
import { useRouter } from "next/navigation";

export default function HomeLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const {accessToken, user} = useGlobalState();
  const router = useRouter();
  
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    setTimeout(() => setLoading(false), 1000);
  }, []);

  useEffect(() => {
    if(accessToken!==""&&user.role !== "Admin") {
        router.push("/")
    }
  }, [accessToken])

  return (
        <div className="dark:bg-boxdark-2 dark:text-bodydark">
          {loading ? (
            <Loader />
          ) : (
            <div className="flex h-screen overflow-hidden">
              {/* <!-- ===== Sidebar Start ===== --> */}
              <Sidebar
                sidebarOpen={sidebarOpen}
                setSidebarOpen={setSidebarOpen}
              />
              {/* <!-- ===== Sidebar End ===== --> */}

              {/* <!-- ===== Content Area Start ===== --> */}
              <div className="relative flex flex-1 flex-col overflow-y-auto overflow-x-hidden">
                {/* <!-- ===== Header Start ===== --> */}
                <Header
                  sidebarOpen={sidebarOpen}
                  setSidebarOpen={setSidebarOpen}
                />
                {/* <!-- ===== Header End ===== --> */}

                {/* <!-- ===== Main Content Start ===== --> */}
                <main>
                  <div className="mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10">
                    {children}
                  </div>
                </main>
                {/* <!-- ===== Main Content End ===== --> */}
              </div>
              {/* <!-- ===== Content Area End ===== --> */}
            </div>
          )}
        </div>
  );
}
