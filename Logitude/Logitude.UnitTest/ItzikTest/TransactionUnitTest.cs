using Logitude.UnitTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.UnitTest.ItzikTest
{
    [TestClass]
    public class TransactionUnitTest
    {
        
        public void AmbientScope_ALLcomplete_Success()
        {
            using (var scope = new TransactionScope())
            {
                using (var ambientScope = new TransactionScope())
                {
                    ambientScope.Complete();
                }

                scope.Complete();

            }
        }
        
        public void AmbientScope_InnerDispose_ShouldThrow_TransactionAbortedException()
        {


            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                using (var scope = new TransactionScope())
                {
                    using (var ambientScope = new TransactionScope())
                    {

                    }

                    scope.Complete();

                }
            },
    // Assert: Verify the result:
    "The transaction has aborted.");

        }


        
        public void AmbientScope_InnerThrowExceptionwithoutComplete_ShouldThrow_TransactionAbortedException()
        {


            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        using (var ambientScope = new TransactionScope())
                        {
                            throw new Exception("do not ambientScope.complete !!");
                            ambientScope.Complete();
                        }

                    }
                    catch (Exception)
                    {


                    }

                    scope.Complete();

                }
            },
    // Assert: Verify the result:
    "The transaction has aborted.");

        }


        [TestMethod]
        public void WhenRequestsheet_1_ScopeNotComplete_CreateNewTransToAbandonQueue_AvoidCrush
            ()
        {
            TestRequestSheetSincrio(true);
        }
        [TestMethod]
        public void WhenRequestsheet_2_ScopeComplete_QueueTransComplete_NormalBeheviour
           ()
        {
            TestRequestSheetSincrio(false);
        }

        private static void TestRequestSheetSincrio(bool Requestsheet_ScopeFailed)
        {
            bool Requestsheet_Scope_Success = true;
            // Arrange
            using (var Queue_scope = new TransactionScope())
            {
                string token = GetTokenQueue();
                try
                {
                    using (var Requestsheet_Scope = new TransactionScope())
                    {
                        if (Requestsheet_ScopeFailed)
                        {
                            throw new Exception("do not ambientScope.complete !!");
                        }
                        Requestsheet_Scope.Complete();
                    }

                }
                catch (Exception)
                {

                    Requestsheet_Scope_Success = false;
                }
                if (Requestsheet_Scope_Success)
                {
                    DBQueue_Complete(token);
                    Queue_scope.Complete();
                }
                else
                {
                    using (var Abandon_Queue_scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        DBQueue_Abandon(token);
                        Abandon_Queue_scope.Complete();
                    }

                }

            }
        }

        private static string GetTokenQueue()
        {
            return "";
        }

        private static void DBQueue_Abandon(string token)
        {
            
        }

        private static void DBQueue_Complete(string token)
        {
            
        }

        
        public void RequiresNewScope_InnerThrowExceptionwithoutComplete_Should_NNNOOOTTT_Throw_TransactionAbortedException()
        {



            // Arrange
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (var ambientScope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        throw new Exception("do not ambientScope.complete !!");
                        ambientScope.Complete();
                    }

                }
                catch (Exception)
                {


                }

                scope.Complete();

            }


        }
        
        public void When_some_transaction_disposes_without_error_or_complete_ShouldThrow_TransactionAbortedException()
        {
            // entering an ambient transaction
            new TransactionScope();

            // start a nested transaction
            var t2 = new TransactionScope();
            // no complete, no exception
            // cancels ambient transaction, too
            t2.Dispose();
            

            //createAScope.ShouldThrow<TransactionAbortedException>().Message.ShouldEqual("The transaction has aborted.");
            // creating a new scope fails
            
            TestsUtil.AssertThrows<Exception>(delegate
            {
                // Arrange
                new TransactionScope();
            },
                // Assert: Verify the result:
                "The transaction has aborted.");
        }
        
        
    }
}

    
