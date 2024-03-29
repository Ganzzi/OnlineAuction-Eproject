"use client";
import { useState, useEffect } from "react";
import Loader from "@/components/common/Loader";
import { useGlobalState } from "@/context/globalState";
import { useRouter } from "next/navigation";

export default function HomeLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const {isLoggedIn, user} = useGlobalState();
  const router = useRouter();

  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    setTimeout(() => setLoading(false), 1000);
  }, []);

  useEffect(() => {
    if(isLoggedIn) {
      if (user.role === "Admin") {
        router.push("/dashboard")
      } else {
        router.push("/")
      }
    }
  }, [isLoggedIn])

  return (
        <div className="dark:bg-boxdark bg-white" style={{
          height: '100vh'
        }}>
          {loading ? (
            <Loader />
          ) : (
            <div className="flex flex-1 flex-col overflow-y-auto overflow-x-hidden">
                <main>
                  <div className="mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10">
                    {children}
                  </div>
                </main>
              </div>
          )}
        </div>
  );
}
