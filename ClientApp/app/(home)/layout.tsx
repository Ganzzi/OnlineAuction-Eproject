"use client";
import "../globals.css";
import "../data-tables-css.css";
import "../satoshi.css";
import { useState, useEffect } from "react";
import Loader from "@/components/common/Loader";

import Header from "@/components/Home/Header";
import { GlobalStateProvider, useGlobalState } from "@/context/globalState";
import Footer from "@/components/Home/Footer";
import { useRouter } from "next/navigation";

export default function HomeLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const {user, isLoggedIn} = useGlobalState();
  const router = useRouter();

  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    setTimeout(() => setLoading(false), 2000);
  }, []);

  useEffect(() => {
    if (isLoggedIn) {
      if (user.userId != 0 && user.role === "Admin") {
        router.push("/dashboard")
      }
    }
  }, [user.userId])

  return (
    <div className="dark:bg-boxdark-2 dark:text-bodydark">
    {loading ? (
      <Loader />
    ) : (
      <div className="flex h-screen overflow-hidden ">
        {/* <!-- ===== Sidebar End ===== --> */}

        {/* <!-- ===== Content Area Start ===== --> */}
        <div className="relative flex flex-1 flex-col overflow-y-auto overflow-x-hidden ">
          {/* <!-- ===== Header Start ===== --> */}
          <Header />
          {/* <!-- ===== Header End ===== --> */}

          {/* <!-- ===== Main Content Start ===== --> */}
          <main>
            <div className="mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10">
              {children}
            </div>
          </main>
          {/* <!-- ===== Main Content End ===== --> */}

          {/* <!-- ===== Footer Start ===== --> */}
          <Footer />
          {/* <!-- ===== Footer End ===== --> */}
        </div>
        {/* <!-- ===== Content Area End ===== --> */}
      </div>
    )}
  </div>
  );
}
